using PrintJobInterceptor.Presentation;
using PrintJobInterceptor.UI.Helpers;
using PrintJobInterceptor.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PrintJobInterceptor.Core.Models;
using PrintJobInterceptor.Core.Interfaces;

namespace PrintJobInterceptor
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly MainFormPresenter _presenter;
        private PrintJobGroup _selectedGroup;

        // Inject the service, construct the presenter here to break the DI cycle
        public MainForm(IPrintJobService printJobService)
        {
            InitializeComponent();
            _presenter = new MainFormPresenter(this, printJobService);
            this.dgvPrintJobs.SelectionChanged += DgvJobGroups_SelectionChanged;
            this.Load += MainForm_Load;

            this.btnPause.Click += (s, e) => PauseJobRequested?.Invoke(GetSelectedJobGroup());
            this.btnResume.Click += (s, e) => ResumeJobRequested?.Invoke(GetSelectedJobGroup());
            this.btnCancel.Click += (s, e) => CancelJobRequested?.Invoke(GetSelectedJobGroup());
        
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _presenter.Start();
        }
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

        public void ShowNotification(string message, FeedbackType type)
        {
        }

        public event Action<PrintJobGroup> PauseJobRequested;
        public event Action<PrintJobGroup> ResumeJobRequested;
        public event Action<PrintJobGroup> CancelJobRequested;
    }
}
