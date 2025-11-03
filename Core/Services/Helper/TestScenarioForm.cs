using Microsoft.Extensions.Logging;
using PrintJobInterceptor.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintJobInterceptor.Core.Services.Helper
{
    public partial class TestScenarioForm : Form
    {
        private readonly MainFormPresenter _presenter;
        private readonly ILogger<TestScenarioForm> _logger;

        public TestScenarioForm(MainFormPresenter presenter, ILogger<TestScenarioForm> logger)
        {
            InitializeComponent();
            _presenter = presenter;
            _logger = logger;
        }

        private void TestScenarioForm_Load(object sender, EventArgs e)
        {
            comboScenarios.DataSource = Enum.GetValues(typeof(TestScenario));
            comboScenarios.DropDownStyle = ComboBoxStyle.DropDownList;
            Debug.WriteLine("TestScenarioForm loaded. Scenarios populated.");

        }
        private void btnRunTest_Click(object sender, EventArgs e)
        {
            if (comboScenarios.SelectedItem != null)
            {
                var selectedScenario = (TestScenario)comboScenarios.SelectedItem;
                _logger.LogInformation("RunTest button clicked. Requesting: {Scenario}", selectedScenario);
                _presenter.RunTestRequested(selectedScenario);
            }
        }
    }
}
