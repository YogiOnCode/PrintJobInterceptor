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
            _presenter = new MainFormPresenter(this, printJobService);

            this.Text = string.Empty; 
            this.ControlBox = false;  
            this.DoubleBuffered = true;

            this.dgvPrintJobs.SelectionChanged += DgvJobGroups_SelectionChanged;
            this.Load += MainForm_Load;

            this.btnPause.Click += (s, e) => PauseJobRequested?.Invoke(GetSelectedJobGroup());
            this.btnResume.Click += (s, e) => ResumeJobRequested?.Invoke(GetSelectedJobGroup());
            this.btnCancel.Click += (s, e) => CancelJobRequested?.Invoke(GetSelectedJobGroup());

            this.btnClose.Click += BtnClose_Click;
            this.btnMinimize.Click += BtnMinimize_Click;
            this.panelTitleBar.MouseDown += PanelTitleBar_MouseDown;
            this.panelTitleBar.MouseMove += PanelTitleBar_MouseMove;
            this.panelTitleBar.MouseUp += PanelTitleBar_MouseUp;
            this.dropDownPrinters.SelectedIndexChanged += DropDownPrinters_SelectedIndexChanged;


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
    }
}
