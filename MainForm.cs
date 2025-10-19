using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI.Helpers;
using PrintJobInterceptor.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.Core.Interfaces;
using System.Drawing.Printing;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace PrintJobInterceptor
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly MainFormPresenter _presenter;
        private PrintJobGroup _selectedGroup;

        private bool isDragging = false;
        private Point lastLocation;

        public event Action<string> PrinterFilterChanged;
        public MainForm(IPrintJobService printJobService)
        {
            InitializeComponent();
            SetupDataGridView(dgvPrintJobs);
            SetupDataGridView(dgvHistoryJobs);
            SetDoubleBuffering(dgvHistoryJobs, true);

            _presenter = new MainFormPresenter(this, printJobService);

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;

            this.dgvPrintJobs.SelectionChanged += DgvJobGroups_SelectionChanged;
            this.Load += MainForm_Load;

            this.buttonPause.Click += (s, e) => PauseJobRequested?.Invoke(GetSelectedJobGroup());
            this.buttonResume.Click += (s, e) => ResumeJobRequested?.Invoke(GetSelectedJobGroup());
            this.buttonCancel.Click += (s, e) => CancelJobRequested?.Invoke(GetSelectedJobGroup());

            this.btnClose.Click += BtnClose_Click;
            this.btnMinimize.Click += BtnMinimize_Click;
            this.panelTitleBar.MouseDown += PanelTitleBar_MouseDown;
            this.panelTitleBar.MouseMove += PanelTitleBar_MouseMove;
            this.panelTitleBar.MouseUp += PanelTitleBar_MouseUp;
            this.dropDownPrinters.SelectedIndexChanged += DropDownPrinters_SelectedIndexChanged;
            tabControlContent.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            this.btnRefresh.Click += BtnRefresh_Click;


        }
        #region Title Bar and Form Events
        private void MainForm_Load(object sender, EventArgs e)
        {
            PopulatePrinterList();
            _presenter.Start();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _presenter.Stop();
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion


        #region Events
        private PrintJobGroup GetSelectedJobGroup()
        {
            if (dgvPrintJobs.CurrentRow != null && dgvPrintJobs.CurrentRow.DataBoundItem is PrintJobGroup selectedGroup)
            {
                return selectedGroup;
            }
            return null;
        }
        private void DgvJobGroups_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrintJobs.CurrentRow != null && dgvPrintJobs.CurrentRow.DataBoundItem is PrintJobGroup selectedGroup)
            {
                _selectedGroup = selectedGroup;

            }
        }
        private int GetSelectedJobId()
        {

            return _selectedGroup?.Jobs.FirstOrDefault()?.JobId ?? 0;
        }

        public void DisplayJobGroups(IEnumerable<PrintJobGroup> groups)
        {
            if (dgvPrintJobs.InvokeRequired)
            {
                dgvPrintJobs.Invoke(new Action(() => DisplayJobGroups(groups)));
                return;
            }


            var columnWidths = new Dictionary<string, int>();
            foreach (DataGridViewColumn column in dgvPrintJobs.Columns)
            {
                columnWidths[column.Name] = column.Width;
            }


            string selectedGroupKey = null;
            if (dgvPrintJobs.CurrentRow != null && dgvPrintJobs.CurrentRow.DataBoundItem is PrintJobGroup currentGroup)
            {
                selectedGroupKey = currentGroup.GroupKey;
            }


            var groupList = groups.ToList();
            dgvPrintJobs.DataSource = null;
            dgvPrintJobs.DataSource = groupList;


            foreach (DataGridViewColumn column in dgvPrintJobs.Columns)
            {
                if (columnWidths.ContainsKey(column.Name))
                {
                    column.Width = columnWidths[column.Name];
                }
            }


            if (selectedGroupKey != null)
            {
                var rowToSelect = dgvPrintJobs.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(row => (row.DataBoundItem as PrintJobGroup)?.GroupKey == selectedGroupKey);

                if (rowToSelect != null)
                {
                    dgvPrintJobs.ClearSelection();
                    rowToSelect.Selected = true;
                    dgvPrintJobs.CurrentCell = rowToSelect.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                }
            }

        }
        #endregion
        #region UI events
        private void PopulatePrinterList()
        {

            dropDownPrinters.Items.Clear();
            dropDownPrinters.Items.Add("All Printers");


            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                dropDownPrinters.Items.Add(printerName);
            }


            dropDownPrinters.SelectedIndex = 0;
        }
        private void DropDownPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPrinter = dropDownPrinters.SelectedItem?.ToString();
            PrinterFilterChanged?.Invoke(selectedPrinter);
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControlContent.SelectedTab == historyTab) // Assuming your history tab page is named historyTab
            {
                LoadHistory();
            }
        }
        private void LoadHistory()
        {
            dgvHistoryJobs.SuspendLayout();
            string historyFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PrintJobInterceptor",
                "history.jsonl");

            if (!File.Exists(historyFile))
            {
                dgvHistoryJobs.ResumeLayout();
                return;
            }

            var historyGroups = new List<PrintJobGroup>();
            var lines = File.ReadAllLines(historyFile);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {

                    var group = JsonConvert.DeserializeObject<PrintJobGroup>(line);
                    if (group != null)
                    {
                        historyGroups.Add(group);
                    }
                }
                catch (JsonException ex)
                {

                    Console.WriteLine($"Error reading history line: {ex.Message}");
                }
            }

            dgvHistoryJobs.DataSource = null;
            dgvHistoryJobs.DataSource = historyGroups.OrderByDescending(g => g.LastActivity).ToList();
            dgvHistoryJobs.ResumeLayout();
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {

            LoadHistory();
        }
        #endregion
        public void ShowNotification(string message, FeedbackType type)
        {
        }

        public event Action<PrintJobGroup> PauseJobRequested;
        public event Action<PrintJobGroup> ResumeJobRequested;
        public event Action<PrintJobGroup> CancelJobRequested;

        #region FormDrag
        private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastLocation = e.Location;
            }
        }


        private void PanelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X,
                    (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void PanelTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
        #endregion
        #region UI
        private void SetupDataGridView(DataGridView dgv)
        {
            dgvPrintJobs.AutoGenerateColumns = false;


            dgvPrintJobs.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Printer",
                DataPropertyName = "PrinterName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "User",
                DataPropertyName = "User"
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Document Name",
                DataPropertyName = "DocumentName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                DataPropertyName = "DocumentType"
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Pages",
                DataPropertyName = "TotalPages"
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Jobs",
                DataPropertyName = "JobCount"
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = "Status"
            });
            dgvPrintJobs.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Last Activity",
                DataPropertyName = "LastActivity",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "g" }
            });
            StyleDataGridView(dgv);
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            var baseColor = Color.FromArgb(34, 34, 34);
            var gridColor = Color.FromArgb(45, 45, 45);
            var textColor = Color.FromArgb(220, 220, 220);
            var selectionColor = Color.FromArgb(55, 55, 55);


            dgvPrintJobs.EnableHeadersVisualStyles = false;


            dgvPrintJobs.BorderStyle = BorderStyle.None;
            dgvPrintJobs.BackgroundColor = baseColor;
            dgvPrintJobs.GridColor = gridColor;
            dgvPrintJobs.RowHeadersVisible = false;


            var segoeFont = new Font("Segoe UI Variable", 9F);
            dgvPrintJobs.Font = segoeFont;


            dgvPrintJobs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPrintJobs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 32, 32);
            dgvPrintJobs.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgvPrintJobs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Variable", 10F, FontStyle.Bold);
            dgvPrintJobs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPrintJobs.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 0, 0);



            dgvPrintJobs.DefaultCellStyle.BackColor = baseColor;
            dgvPrintJobs.DefaultCellStyle.ForeColor = textColor;
            dgvPrintJobs.DefaultCellStyle.SelectionBackColor = selectionColor;
            dgvPrintJobs.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPrintJobs.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPrintJobs.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);



            dgvPrintJobs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrintJobs.AllowUserToResizeRows = false;
            dgvPrintJobs.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvPrintJobs.RowTemplate.Height = 35;
        }
        public static void SetDoubleBuffering(System.Windows.Forms.Control control, bool value)
        {

            typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(control, value, null);
        }
        #endregion


       
    }
}
