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

        // Inject the service, construct the presenter here to break the DI cycle
        public MainForm(IPrintJobService printJobService)
        {
            InitializeComponent();
            _presenter = new MainFormPresenter(this, printJobService);
            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _presenter.Start();
        }

        public void DisplayJobGroups(IEnumerable<PrintJobGroup> groups)
        {
            if (dgvPrintJobs.InvokeRequired)
            {
                dgvPrintJobs.Invoke(new Action(() => DisplayJobGroups(groups)));
                return;
            }

            // Bind the list of groups to the DataGridView
            dgvPrintJobs.DataSource = null;
            dgvPrintJobs.DataSource = groups.ToList();
        }

        public void ShowNotification(string message, FeedbackType type)
        {
        }

        public event Action<int> PauseJobRequested;
        public event Action<int> ResumeJobRequested;
        public event Action<int> CancelJobRequested;
    }
}
