using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsLinuxDemo;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ComprehensiveTestForm());
    }
}

public class ComprehensiveTestForm : Form
{
    private TabControl mainTabControl = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;

    public ComprehensiveTestForm()
    {
        this.Text = "Linux WinForms Feature & Primitive Tester";
        this.Size = new Size(800, 480);
        this.MinimumSize = new Size(800, 480);
        this.StartPosition = FormStartPosition.CenterScreen;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // 1. Status Bar Setup
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel("Ready - Environment initialized.");
        statusStrip.Items.Add(statusLabel);
        this.Controls.Add(statusStrip);

        // 2. Tab Control Setup
        mainTabControl = new TabControl { Dock = DockStyle.Fill };
        
        mainTabControl.TabPages.Add(CreateBasicControlsTab());
        mainTabControl.TabPages.Add(CreateDataAndListsTab());
        mainTabControl.TabPages.Add(CreateDialogsAndMessagesTab());
        mainTabControl.TabPages.Add(CreateCustomDrawingTab());
        mainTabControl.TabPages.Add(CreateHmiDashboardTab());
        mainTabControl.TabPages.Add(CreateAnimatedGdiLinesTab());

        this.Controls.Add(mainTabControl);
    }

    // --- TAB 1: Basic Input Controls ---
    private TabPage CreateBasicControlsTab()
    {
        var tab = new TabPage("Basic Controls");
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(15),
            FlowDirection = FlowDirection.TopDown
        };

        // Text & Buttons
        var btn = new Button { Text = "Standard Button", Width = 150 };
        btn.Click += (s, e) => LogStatus("Button Clicked");

        var chk = new CheckBox { Text = "Enable Checkbox Feature", AutoSize = true };
        chk.CheckedChanged += (s, e) => LogStatus($"Checkbox State: {chk.Checked}");

        var txt = new TextBox { Text = "Editable Textbox", Width = 200 };
        
        // Progress Bar & TrackBar
        var progress = new ProgressBar { Value = 45, Width = 200 };
        var track = new TrackBar { Minimum = 0, Maximum = 100, Value = 45, Width = 200 };
        track.ValueChanged += (s, e) => {
            progress.Value = track.Value;
            LogStatus($"Trackbar set to: {track.Value}");
        };

        // DateTimePicker
        var dtp = new DateTimePicker { Width = 200 };
        dtp.ValueChanged += (s, e) => LogStatus($"Date selected: {dtp.Value.ToShortDateString()}");

        panel.Controls.AddRange([
            new Label { Text = "Input Primitives", Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) },
            btn, chk, txt,
            new Label { Text = "Progress & Sliders", Margin = new Padding(0, 10, 0, 0) },
            track, progress,
            new Label { Text = "Date Picker", Margin = new Padding(0, 10, 0, 0) },
            dtp
        ]);

        tab.Controls.Add(panel);
        return tab;
    }

    // --- TAB 2: Data, Lists & Trees ---
    private TabPage CreateDataAndListsTab()
    {
        var tab = new TabPage("Data & Lists");
        var splitContainer = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 450 };

        // TreeView (Left)
        var tree = new TreeView { Dock = DockStyle.Fill };
        var rootNode = new TreeNode("System Node");
        rootNode.Nodes.Add("Child Component 1");
        rootNode.Nodes.Add("Child Component 2");
        tree.Nodes.Add(rootNode);
        tree.ExpandAll();
        tree.AfterSelect += (s, e) => LogStatus($"Tree Selected: {e.Node?.Text}");

        // DataGridView (Right)
        var grid = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = true };
        var table = new System.Data.DataTable("SampleData");
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Feature Name", typeof(string));
        table.Columns.Add("Status", typeof(string));

        table.Rows.Add(1, "X11 Windowing", "Active");
        table.Rows.Add(2, "GDI+ Drawing", "Active");
        table.Rows.Add(3, "Event Dispatching", "Active");

        grid.DataSource = table;

        splitContainer.Panel1.Controls.Add(tree);
        splitContainer.Panel2.Controls.Add(grid);

        tab.Controls.Add(splitContainer);
        return tab;
    }

    // --- TAB 3: Dialogs & Message Boxes ---
    private TabPage CreateDialogsAndMessagesTab()
    {
        var tab = new TabPage("Dialogs & Messages");
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(15) };

        // MessageBox Tests
        var btnMsgInfo = new Button { Text = "Show Info Box", Width = 160 };
        btnMsgInfo.Click += (s, e) => MessageBox.Show(
            "This is a standard information message box.", 
            "Information", 
            MessageBoxButtons.OK, 
            MessageBoxIcon.Information);

        var btnMsgConfirm = new Button { Text = "Show Confirm Box", Width = 160 };
        btnMsgConfirm.Click += (s, e) => {
            var res = MessageBox.Show(
                "Do you want to accept this choice?", 
                "Question", 
                MessageBoxButtons.YesNoCancel, 
                MessageBoxIcon.Question);
            LogStatus($"Dialog Result: {res}");
        };

        // File Dialogs
        var btnFile = new Button { Text = "Open File Dialog", Width = 160 };
        btnFile.Click += (s, e) => {
            using var dlg = new OpenFileDialog { Filter = "All Files (*.*)|*.*" };
            if (dlg.ShowDialog() == DialogResult.OK)
                LogStatus($"Selected File: {dlg.FileName}");
        };

        // Color Dialog
        var btnColor = new Button { Text = "Color Picker Dialog", Width = 160 };
        btnColor.Click += (s, e) => {
            using var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                LogStatus($"Color Picked: {dlg.Color}");
        };

        panel.Controls.AddRange([btnMsgInfo, btnMsgConfirm, btnFile, btnColor]);
        tab.Controls.Add(panel);
        return tab;
    }

    // --- TAB 4: Custom Canvas Drawing (GDI+ Primitive Test) ---
    private TabPage CreateCustomDrawingTab()
    {
        var tab = new TabPage("GDI+ Graphics Canvas");
        var canvas = new PictureBox { Dock = DockStyle.Fill, BackColor = Color.White };

        canvas.Paint += (s, e) => {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Lines & Shapes
            using (var pen = new Pen(Color.Navy, 3))
                g.DrawRectangle(pen, 20, 20, 150, 100);

            using (var brush = new SolidBrush(Color.CornflowerBlue))
                g.FillEllipse(brush, 200, 20, 120, 120);

            // Custom Text Rendering
            using (var font = new Font("Sans", 12, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.DarkGreen))
                g.DrawString("Native GDI+ Drawing Test", font, textBrush, new PointF(20, 160));
        };

        tab.Controls.Add(canvas);
        return tab;
    }

    private TabPage CreateHmiDashboardTab()
    {
        var tab = new TabPage("Boiler HMI");
        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            BackColor = Color.FromArgb(12, 18, 28),
            ColumnCount = 3,
            RowCount = 2
        };

        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));

        var boilerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(20, 30, 39),
            BorderStyle = BorderStyle.FixedSingle
        };

        var boilerLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(12),
            AutoScroll = true,
            BackColor = Color.FromArgb(20, 30, 39)
        };

        var boilerTitle = new Label
        {
            Text = "Boiler Overview",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = Color.AliceBlue,
            AutoSize = true
        };

        var boilerStatus = new Label
        {
            Text = "System Stable",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.LimeGreen,
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 0)
        };

        var boilerTempValue = new Label
        {
            Text = "Temperature: 42.0°C",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.Orange,
            AutoSize = true,
            Margin = new Padding(0, 10, 0, 0)
        };

        var boilerPressureValue = new Label
        {
            Text = "Pressure: 1.2 bar",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.DeepSkyBlue,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 0)
        };

        var boilerFlowValue = new Label
        {
            Text = "Flow: 35/28 L/min",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.PowderBlue,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 0)
        };

        var boilerHeaterValue = new Label
        {
            Text = "Heater: ON",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.OrangeRed,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 0)
        };

        float boilerTemp = 42f;
        float pressure = 1.2f;
        float inputFlow = 35f;
        float outputFlow = 28f;
        float heaterPower = 60f;
        float targetTemp = 75f;

        var boilerVisual = new Panel
        {
            Height = 170,
            Width = 260,
            Margin = new Padding(0, 16, 0, 0),
            BackColor = Color.FromArgb(10, 15, 22),
            BorderStyle = BorderStyle.FixedSingle
        };

        boilerVisual.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var bodyRect = new Rectangle(35, 24, 190, 110);
            using var bodyBrush = new SolidBrush(Color.FromArgb(60, 60, 70));
            using var borderPen = new Pen(Color.FromArgb(140, 180, 210), 2f);
            using var pipePen = new Pen(Color.FromArgb(70, 120, 150), 3f);
            using var waterBrush = new SolidBrush(Color.FromArgb(40, 150, 220));

            g.FillRectangle(bodyBrush, bodyRect);
            g.DrawRectangle(borderPen, bodyRect);

            var waterHeight = (int)(Math.Clamp(boilerTemp / 180f, 0, 1) * 80f);
            var waterRect = new Rectangle(bodyRect.Left + 15, bodyRect.Bottom - 12 - waterHeight, bodyRect.Width - 30, waterHeight);
            g.FillRectangle(waterBrush, waterRect);

            g.DrawLine(pipePen, 15, 60, 35, 60);
            g.DrawLine(pipePen, 225, 60, 245, 60);

            var flowScale = Math.Clamp((inputFlow - 5f) / 80f, 0f, 1f);
            var outScale = Math.Clamp((outputFlow - 5f) / 80f, 0f, 1f);
            using var flowBlue = new SolidBrush(Color.FromArgb(120, 200, 255));
            using var flowRed = new SolidBrush(Color.FromArgb(255, 120, 120));
            g.FillRectangle(flowBlue, new Rectangle(15, 52, (int)(26 * flowScale), 16));
            g.FillRectangle(flowRed, new Rectangle(245 - (int)(26 * outScale), 52, (int)(26 * outScale), 16));

            using var heaterBrush = new SolidBrush(heaterPower > 0.1f ? Color.Orange : Color.Gray);
            g.FillEllipse(heaterBrush, new Rectangle(bodyRect.Right - 32, 42, 20, 20));

            using var textBrush = new SolidBrush(Color.WhiteSmoke);
            using var font = new Font("Segoe UI", 10F, FontStyle.Bold);
            g.DrawString("INPUT", font, textBrush, new PointF(8f, 38f));
            g.DrawString("OUTPUT", font, textBrush, new PointF(216f, 38f));
            g.DrawString("HEATER", font, textBrush, new PointF(146f, 16f));
        };

        boilerLayout.Controls.AddRange([
            boilerTitle,
            boilerStatus,
            boilerTempValue,
            boilerPressureValue,
            boilerFlowValue,
            boilerHeaterValue,
            boilerVisual
        ]);

        boilerPanel.Controls.Add(boilerLayout);

        var tempGauge = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle };
        var pressureGauge = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle };
        var trendViewer = new PictureBox { Dock = DockStyle.Fill, BackColor = Color.FromArgb(7, 11, 19), BorderStyle = BorderStyle.FixedSingle };

        var controlsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(12),
            BackColor = Color.FromArgb(20, 30, 39)
        };

        var controlTitles = new Label
        {
            Text = "Control Inputs",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true
        };

        var targetTempLabel = new Label { Text = "Target Temp: 75°C", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.Orange, AutoSize = true };
        var targetTempSlider = new TrackBar { Width = 220, Minimum = 35, Maximum = 120, Value = 75, TickFrequency = 5 };
        targetTempSlider.ValueChanged += (s, e) => targetTempLabel.Text = $"Target Temp: {targetTempSlider.Value}°C";

        var inputFlowLabel = new Label { Text = "Input Flow: 35 L/min", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.DeepSkyBlue, AutoSize = true };
        var inputFlowSlider = new TrackBar { Width = 220, Minimum = 10, Maximum = 90, Value = 35, TickFrequency = 5 };
        inputFlowSlider.ValueChanged += (s, e) => inputFlowLabel.Text = $"Input Flow: {inputFlowSlider.Value} L/min";

        var outputFlowLabel = new Label { Text = "Output Flow: 28 L/min", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.IndianRed, AutoSize = true };
        var outputFlowSlider = new TrackBar { Width = 220, Minimum = 10, Maximum = 90, Value = 28, TickFrequency = 5 };
        outputFlowSlider.ValueChanged += (s, e) => outputFlowLabel.Text = $"Output Flow: {outputFlowSlider.Value} L/min";

        var heaterPowerLabel = new Label { Text = "Heater Power: 60%", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.OrangeRed, AutoSize = true };
        var heaterPowerSlider = new TrackBar { Width = 220, Minimum = 0, Maximum = 100, Value = 60, TickFrequency = 10 };
        heaterPowerSlider.ValueChanged += (s, e) => heaterPowerLabel.Text = $"Heater Power: {heaterPowerSlider.Value}%";

        controlsPanel.Controls.AddRange([
            controlTitles,
            targetTempLabel,
            targetTempSlider,
            inputFlowLabel,
            inputFlowSlider,
            outputFlowLabel,
            outputFlowSlider,
            heaterPowerLabel,
            heaterPowerSlider
        ]);

        var tempSeries = new List<float>();
        var pressureSeries = new List<float>();
        var timer = new System.Windows.Forms.Timer { Interval = 110 };

        void UpdateDashboard()
        {
            inputFlow = inputFlowSlider.Value;
            outputFlow = outputFlowSlider.Value;
            heaterPower = heaterPowerSlider.Value;
            targetTemp = targetTempSlider.Value;

            var tempDelta = (heaterPower * 0.12f) - (outputFlow * 0.08f) - (inputFlow * 0.025f) + ((targetTemp - boilerTemp) * 0.06f);
            boilerTemp = Math.Clamp(boilerTemp + tempDelta, 18f, 180f);
            pressure = Math.Clamp(pressure + (inputFlow - outputFlow) * 0.03f + (heaterPower * 0.012f) - 0.08f, 0.2f, 14f);

            if (boilerTemp >= targetTemp + 4f)
            {
                boilerStatus.Text = "Overtemp Warning";
                boilerStatus.ForeColor = Color.OrangeRed;
            }
            else if (boilerTemp >= targetTemp - 2f)
            {
                boilerStatus.Text = "Temperature Locked";
                boilerStatus.ForeColor = Color.Gold;
            }
            else
            {
                boilerStatus.Text = "System Stable";
                boilerStatus.ForeColor = Color.LimeGreen;
            }

            boilerTempValue.Text = $"Temperature: {boilerTemp:0.0}°C";
            boilerPressureValue.Text = $"Pressure: {pressure:0.0} bar";
            boilerFlowValue.Text = $"Flow: {inputFlow:0.0}/{outputFlow:0.0} L/min";
            boilerHeaterValue.Text = heaterPower > 0 ? "Heater: ON" : "Heater: OFF";
            boilerHeaterValue.ForeColor = heaterPower > 0 ? Color.OrangeRed : Color.Gray;

            tempSeries.Add(boilerTemp);
            pressureSeries.Add(pressure);

            if (tempSeries.Count > 52)
            {
                tempSeries.RemoveAt(0);
                pressureSeries.RemoveAt(0);
            }

            tempGauge.Invalidate();
            pressureGauge.Invalidate();
            trendViewer.Invalidate();
            boilerVisual.Invalidate();
        }

        void DrawGauge(Panel gauge, PaintEventArgs e, float value, float min, float max, string title, string unit, Color needleColor)
        {
            var g = e.Graphics;
            g.Clear(gauge.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var cx = gauge.Width / 2f;
            var cy = gauge.Height - 24f;
            var radius = Math.Min(gauge.Width, gauge.Height - 30) * 0.36f;
            var startAngle = 200f;
            var sweepAngle = 140f;

            using var arcPen = new Pen(Color.FromArgb(80, 130, 210), 10f);
            using var tickPen = new Pen(Color.FromArgb(220, 220, 220), 1.5f);
            using var needlePen = new Pen(needleColor, 4f);
            using var centerBrush = new SolidBrush(Color.FromArgb(30, 40, 52));
            using var titleBrush = new SolidBrush(Color.Silver);
            using var valueBrush = new SolidBrush(Color.White);
            using var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            using var valueFont = new Font("Segoe UI", 18F, FontStyle.Bold);

            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);
            g.DrawArc(arcPen, arcRect, startAngle, sweepAngle);

            for (var i = 0; i <= 10; i++)
            {
                var t = i / 10f;
                var angle = (startAngle + t * sweepAngle) * Math.PI / 180.0;
                var inner = radius - 12f;
                var outer = radius + 2f;
                var x1 = (float)(cx + inner * Math.Cos(angle));
                var y1 = (float)(cy + inner * Math.Sin(angle));
                var x2 = (float)(cx + outer * Math.Cos(angle));
                var y2 = (float)(cy + outer * Math.Sin(angle));
                g.DrawLine(tickPen, x1, y1, x2, y2);
            }

            var ratio = Math.Clamp((value - min) / (max - min), 0f, 1f);
            var needleAngle = (startAngle + ratio * sweepAngle) * Math.PI / 180.0;
            var needleLength = radius - 20f;
            var needleX = (float)(cx + needleLength * Math.Cos(needleAngle));
            var needleY = (float)(cy + needleLength * Math.Sin(needleAngle));
            g.DrawLine(needlePen, cx, cy, needleX, needleY);

            g.FillEllipse(centerBrush, cx - 18, cy - 18, 36, 36);
            g.DrawEllipse(new Pen(Color.FromArgb(170, 170, 170), 2f), cx - 18, cy - 18, 36, 36);

            g.DrawString(title, titleFont, titleBrush, new PointF(cx - 22f, 12f));
            using var format = new StringFormat { Alignment = StringAlignment.Center };
            var valueRect = new RectangleF(0, gauge.Height - 42, gauge.Width, 28);
            g.DrawString($"{value:0.0}{unit}", valueFont, valueBrush, valueRect, format);

            var lowText = $"{min:0.0}{unit.Trim()}";
            var highText = $"{max:0.0}{unit.Trim()}";
            g.DrawString(lowText, titleFont, titleBrush, new PointF(cx - radius, cy + 12f));
            g.DrawString(highText, titleFont, titleBrush, new PointF(cx + radius - 26f, cy + 12f));
        }

        tempGauge.Paint += (s, e) => DrawGauge(tempGauge, e, boilerTemp, 20f, 180f, "TEMP", "°C", Color.Orange);

        pressureGauge.Paint += (s, e) => DrawGauge(pressureGauge, e, pressure, 0.5f, 14f, "PRESS", " bar", Color.DeepSkyBlue);

        trendViewer.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.Clear(trendViewer.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var gridPen = new Pen(Color.FromArgb(45, 65, 90), 1f);
            using var tempPen = new Pen(Color.Orange, 2f);
            using var pressurePen = new Pen(Color.DeepSkyBlue, 2f);
            using var font = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var labelBrush = new SolidBrush(Color.White);

            for (var i = 0; i < 7; i++)
            {
                var y = 20 + i * ((trendViewer.Height - 40) / 6);
                g.DrawLine(gridPen, 20, y, trendViewer.Width - 20, y);
            }

            for (var i = 0; i < 8; i++)
            {
                var x = 20 + i * ((trendViewer.Width - 40) / 7);
                g.DrawLine(gridPen, x, 20, x, trendViewer.Height - 20);
            }

            if (tempSeries.Count > 1)
            {
                var maxTemp = Math.Max(100f, tempSeries.Max() + 10f);
                var minTemp = Math.Min(0f, tempSeries.Min() - 10f);
                var maxPressure = Math.Max(8f, pressureSeries.Max() + 1f);
                var minPressure = Math.Min(0f, pressureSeries.Min() - 1f);

                for (var i = 1; i < tempSeries.Count; i++)
                {
                    var x1 = 20 + ((i - 1) * (trendViewer.Width - 40)) / Math.Max(1, tempSeries.Count - 1);
                    var y1 = trendViewer.Height - 20 - ((tempSeries[i - 1] - minTemp) * (trendViewer.Height - 40)) / (maxTemp - minTemp);
                    var x2 = 20 + (i * (trendViewer.Width - 40)) / Math.Max(1, tempSeries.Count - 1);
                    var y2 = trendViewer.Height - 20 - ((tempSeries[i] - minTemp) * (trendViewer.Height - 40)) / (maxTemp - minTemp);
                    g.DrawLine(tempPen, x1, y1, x2, y2);
                }

                for (var i = 1; i < pressureSeries.Count; i++)
                {
                    var x1 = 20 + ((i - 1) * (trendViewer.Width - 40)) / Math.Max(1, pressureSeries.Count - 1);
                    var y1 = trendViewer.Height - 20 - ((pressureSeries[i - 1] - minPressure) * (trendViewer.Height - 40)) / (maxPressure - minPressure);
                    var x2 = 20 + (i * (trendViewer.Width - 40)) / Math.Max(1, pressureSeries.Count - 1);
                    var y2 = trendViewer.Height - 20 - ((pressureSeries[i] - minPressure) * (trendViewer.Height - 40)) / (maxPressure - minPressure);
                    g.DrawLine(pressurePen, x1, y1, x2, y2);
                }
            }

            g.DrawString("Temperature", font, labelBrush, new PointF(22f, 4f));
            g.DrawString("Pressure", font, labelBrush, new PointF(120f, 4f));
        };

        tempSeries.Add(boilerTemp);
        pressureSeries.Add(pressure);
        timer.Tick += (s, e) => UpdateDashboard();
        timer.Start();

        rootLayout.Controls.Add(boilerPanel, 0, 0);
        rootLayout.Controls.Add(tempGauge, 1, 0);
        rootLayout.Controls.Add(pressureGauge, 2, 0);
        rootLayout.Controls.Add(trendViewer, 0, 1);
        rootLayout.Controls.Add(controlsPanel, 1, 1);
        rootLayout.SetColumnSpan(controlsPanel, 2);

        tab.Controls.Add(rootLayout);
        return tab;
    }

    private TabPage CreateAnimatedGdiLinesTab()
    {
        var tab = new TabPage("Animated GDI Lines");
        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };

        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var controlsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            Padding = new Padding(10)
        };

        var lineCountLabel = new Label
        {
            Text = "Line count:",
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 0)
        };

        var lineCountSelector = new NumericUpDown
        {
            Width = 80,
            Minimum = 1,
            Maximum = 200,
            Value = 25,
            Margin = new Padding(0, 4, 0, 0)
        };

        var lineCountValueLabel = new Label
        {
            Text = $"{lineCountSelector.Value} lines",
            AutoSize = true,
            Margin = new Padding(8, 6, 0, 0)
        };

        var resetButton = new Button
        {
            Text = "Reset motion",
            Width = 120,
            Margin = new Padding(12, 4, 0, 0)
        };

        controlsPanel.Controls.AddRange([
            lineCountLabel,
            lineCountSelector,
            lineCountValueLabel,
            resetButton
        ]);

        var canvas = new PictureBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        rootLayout.Controls.Add(controlsPanel, 0, 0);
        rootLayout.Controls.Add(canvas, 0, 1);
        tab.Controls.Add(rootLayout);

        var rnd = new Random();
        var animatedLines = new List<BouncingLine>();
        var timer = new System.Windows.Forms.Timer { Interval = 16 };

        void RebuildLines()
        {
            animatedLines.Clear();

            for (var i = 0; i < lineCountSelector.Value; i++)
            {
                var length = rnd.Next(40, 150);
                var startX = rnd.Next(20, Math.Max(21, canvas.Width - 20));
                var startY = rnd.Next(20, Math.Max(21, canvas.Height - 20));
                var angle = (float)(rnd.NextDouble() * Math.PI * 2);
                var speed = rnd.Next(1, 4);

                animatedLines.Add(new BouncingLine
                {
                    X = startX,
                    Y = startY,
                    Length = length,
                    Angle = angle,
                    VelocityX = (float)(Math.Cos(angle) * speed),
                    VelocityY = (float)(Math.Sin(angle) * speed),
                    PenWidth = 1.5f + (i % 4) * 0.5f,
                    Color = Color.FromArgb(160, rnd.Next(40, 220), rnd.Next(40, 220), rnd.Next(40, 220))
                });
            }

            lineCountValueLabel.Text = $"{lineCountSelector.Value} lines";
            canvas.Invalidate();
        }

        lineCountSelector.ValueChanged += (s, e) =>
        {
            RebuildLines();
        };

        resetButton.Click += (s, e) => RebuildLines();

        canvas.Resize += (s, e) => RebuildLines();

        canvas.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            foreach (var line in animatedLines)
            {
                var x2 = line.X + (float)Math.Cos(line.Angle) * line.Length;
                var y2 = line.Y + (float)Math.Sin(line.Angle) * line.Length;

                using var pen = new Pen(line.Color, line.PenWidth);
                g.DrawLine(pen, line.X, line.Y, x2, y2);
            }
        };

        timer.Tick += (s, e) =>
        {
            const float padding = 12f;

            foreach (var line in animatedLines)
            {
                line.X += line.VelocityX;
                line.Y += line.VelocityY;

                if (line.X < padding || line.X > canvas.Width - padding)
                {
                    line.VelocityX *= -1;
                    line.X = Math.Clamp(line.X, padding, canvas.Width - padding);
                }

                if (line.Y < padding || line.Y > canvas.Height - padding)
                {
                    line.VelocityY *= -1;
                    line.Y = Math.Clamp(line.Y, padding, canvas.Height - padding);
                }
            }

            canvas.Invalidate();
        };

        RebuildLines();
        timer.Start();

        return tab;
    }

    private sealed class BouncingLine
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Length { get; set; }
        public float Angle { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float PenWidth { get; set; }
        public Color Color { get; set; } = Color.Black;
    }

    private void LogStatus(string text) => statusLabel.Text = $"[{DateTime.Now:HH:mm:ss}] {text}";
}
