using System.Text;
using Newtonsoft.Json;
using PrintJobInterceptor.Core.Interfaces;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI.Helpers;
using PrintJobInterceptor.UI.Interfaces;
using static SiticoneNetCoreUI.SiticoneDropdown;
using Microsoft.Extensions.Logging;

namespace PrintJobInterceptor
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly MainFormPresenter _presenter;
        private PrintJobGroup _selectedGroup;
        private readonly ILogger<MainForm> _logger;

        private bool isDragging = false;
        private Point lastLocation;

        public event Action<List<string>> PrinterFilterChanged;
        private readonly HashSet<string> _selectedPrinters = new HashSet<string>();
        public MainForm(IPrintJobService printJobService, 
                        ILogger<MainForm> logger,
                        ILogger<MainFormPresenter> presenterLogger)
        {
            InitializeComponent();
            _logger = logger;
            _logger.LogInformation("MainForm component is initializing.");

            SetupDataGridView(dgvPrintJobs);
            SetupDataGridView(dgvHistoryJobs); 
            SetDoubleBuffering(dgvHistoryJobs, true);
           
            _presenter = new MainFormPresenter(this, printJobService, presenterLogger);

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;

            this.dgvPrintJobs.SelectionChanged += DgvJobGroups_SelectionChanged;
            this.Load += MainForm_Load;

            this.buttonPause.Click += (s, e) => PauseJobRequested?.Invoke(GetSelectedJobGroup());
            this.buttonResume.Click += (s, e) => ResumeJobRequested?.Invoke(GetSelectedJobGroup());
            this.buttonCancel.Click += (s, e) => CancelJobRequested?.Invoke(GetSelectedJobGroup());

           
            IntializeWireEvents();
        }

       


        #region Title Bar and Form Events
        private void MainForm_Load(object sender, EventArgs e)
        {
            _logger.LogDebug("MainForm_Load event hit");
            PopulatePrinterList();
            _presenter.Start();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.LogDebug("MainForm_FormClosing event fired");
            _presenter.Stop();
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            _logger.LogDebug("Close button clicked.");
            this.Close();
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            _logger.LogDebug("Minimize button clicked.");
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
            var selectedGroup = GetSelectedJobGroup();
            if (selectedGroup != null)
            {
                _logger.LogDebug("Selection changed. Displaying details for GroupKey: {GroupKey}", selectedGroup.GroupKey);
                _selectedGroup = selectedGroup;


                lblDetailsHeader.Text = selectedGroup.User;


                lblDetailsGlobalStatus.Text = $"Status: {selectedGroup.Status}";


                lblDetailsGroupingStatus.Text = $"Grouping: {selectedGroup.GroupingStatus}";


                lblDetailsJobCount.Text = $"Job Count: {selectedGroup.JobCount}";

                UpdateStatusIconColor(selectedGroup.Status);

                StringBuilder jobNamesBuilder = new StringBuilder();

            
                jobNamesBuilder.AppendLine("Job Document Names:");

               
                foreach (var job in selectedGroup.Jobs.OrderBy(j => j.JobId))
                {
                   
                    jobNamesBuilder.AppendLine($"{job.DocumentName}" +
                        $"\n | Status: {job.Status}");
                }
                lblIndividualJobNames.Text = jobNamesBuilder.ToString();

                panelDetails.Visible = true;
            }
            else
            {
                _logger.LogDebug("Selection cleared.");
                panelDetails.Visible = false;
            }
        }
        private void UpdateStatusIconColor(string status)
        {
           
            if (status.Equals("Finished", StringComparison.OrdinalIgnoreCase) || status.Contains("Completed"))
            {
              
              
                iconStatus.IconColor = Color.FromArgb(0, 192, 0); 
            }
            else if (status.Contains("Error"))
            {

                iconStatus.IconColor = Color.Red;
            }
            else if (status.Contains("Paused") || status.Contains("Cancel") || status.Contains("Deleting"))
            {

                iconStatus.IconColor = Color.FromArgb(255, 128, 0);
            }
            else
            {

                iconStatus.IconColor = Color.FromArgb(0, 192, 192); 
            }
        }

        public void DisplayJobGroups(IEnumerable<PrintJobGroup> groups)
        {
            if (dgvPrintJobs.InvokeRequired)
            {
                _logger.LogTrace("Invoke required for DisplayJobGroups.");
                dgvPrintJobs.Invoke(new Action(() => DisplayJobGroups(groups)));
                return;
            }

            string selectedGroupKey = (dgvPrintJobs.CurrentRow?.DataBoundItem as PrintJobGroup)?.GroupKey;

            var groupList = groups.ToList();
            if (groupList.Count == 0)
            {
                dgvPrintJobs.DataSource = groupList; 
                dgvPrintJobs.ClearSelection();
                panelDetails.Visible = false;
                if (lblIndividualJobNames != null)
                {
                    lblIndividualJobNames.Text = string.Empty;
                }
                return; 
            }
            dgvPrintJobs.DataSource = groupList;

            if (selectedGroupKey != null)
            {
                var rowToSelect = dgvPrintJobs.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(row => (row.DataBoundItem as PrintJobGroup)?.GroupKey == selectedGroupKey);

                if (rowToSelect != null)
                {

                    this.dgvPrintJobs.SelectionChanged -= DgvJobGroups_SelectionChanged;

                    dgvPrintJobs.ClearSelection();
                    rowToSelect.Selected = true;
                    dgvPrintJobs.CurrentCell = rowToSelect.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);

                    this.dgvPrintJobs.SelectionChanged += DgvJobGroups_SelectionChanged;


                    DgvJobGroups_SelectionChanged(this, EventArgs.Empty);
                }
            }


        }
        #endregion
        #region UI events
        private void PopulatePrinterList()
        {
            _logger.LogDebug("Populating printer list.");
            dropDownPrinters.Items.Clear();

            foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                dropDownPrinters.Items.Add(printerName);
            }
        }
        
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControlContent.SelectedTab == historyTab) 
            {
                _logger.LogInformation("History tab selected. Loading history.");
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
                _logger.LogWarning("History file not found: {HistoryFile}", historyFile);
                dgvHistoryJobs.ResumeLayout();
                return;
            }

            var historyGroups = new List<PrintJobGroup>();
            var lines = File.ReadAllLines(historyFile);
            _logger.LogDebug("Reading {LineCount} lines from history file.", lines.Length);

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

                   _logger.LogError(ex, "Failed to deserialize history line: {HistoryLine}", line);
                
                }
            }

            dgvHistoryJobs.DataSource = null;
            dgvHistoryJobs.DataSource = historyGroups.OrderByDescending(g => g.LastActivity).ToList();
            dgvHistoryJobs.ResumeLayout();
            _logger.LogInformation("History loaded successfully. {GroupCount} groups displayed.", historyGroups.Count);
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("History refresh button clicked.");
            LoadHistory();
        }
        private void DropDownPrinters_ItemsSelected(object sender, DropdownItemsSelectedEventArgs e)
        {
            List<string> selectedPrinters = new List<string>();
            if (e.SelectedTexts != null)
            {
                foreach (var text in e.SelectedTexts)
                {
                    selectedPrinters.Add(text);
                }
            }

            _logger.LogDebug("Printer filter items selected, raising event for presenter. Count: {Count}", selectedPrinters.Count);
            PrinterFilterChanged?.Invoke(selectedPrinters);
        }
        private void DropDownPrinters_AfterDropdownClose(object sender, EventArgs e)
        {

            if (dropDownPrinters.SelectedItems.Count() == 0)
            {

                PrinterFilterChanged?.Invoke(new List<string>());
            }
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
        private void IntializeWireEvents()
        {
            this.btnClose.Click += BtnClose_Click;
            this.btnMinimize.Click += BtnMinimize_Click;
            this.panelTitleBar.MouseDown += PanelTitleBar_MouseDown;
            this.panelTitleBar.MouseMove += PanelTitleBar_MouseMove;
            this.panelTitleBar.MouseUp += PanelTitleBar_MouseUp;
            tabControlContent.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            this.btnRefresh.Click += BtnRefresh_Click;
            this.dropDownPrinters.ItemsSelected += DropDownPrinters_ItemsSelected;
            this.dropDownPrinters.AfterDropdownClose += DropDownPrinters_AfterDropdownClose;

        }

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
