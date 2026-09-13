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
        
        mainTabControl.TabPages.Add(CreateHmiDashboardTab());
        mainTabControl.TabPages.Add(CreateBasicControlsTab());
        mainTabControl.TabPages.Add(CreateDataAndListsTab());
        mainTabControl.TabPages.Add(CreateDialogsAndMessagesTab());
        mainTabControl.TabPages.Add(CreateCustomDrawingTab());
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
        var tab = new TabPage("Signal HMI");

        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(12, 18, 28),
            ColumnCount = 4,
            RowCount = 2
        };

        for (var i = 0; i < 4; i++)
        {
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

        var gaugePanels = new[]
        {
            new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle },
            new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle },
            new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle },
            new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(15, 22, 30), BorderStyle = BorderStyle.FixedSingle }
        };

        for (var i = 0; i < gaugePanels.Length; i++)
        {
            rootLayout.Controls.Add(gaugePanels[i], i, 0);
        }

        var trendViewer = new PictureBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(7, 11, 19),
            BorderStyle = BorderStyle.FixedSingle
        };

        rootLayout.Controls.Add(trendViewer, 0, 1);
        rootLayout.SetColumnSpan(trendViewer, 4);

        var maxSamples = 100;
        var sampleQueues = new[]
        {
            new Queue<float>(),
            new Queue<float>(),
            new Queue<float>(),
            new Queue<float>()
        };

        var currentValues = new float[4];
        var startTime = DateTime.UtcNow;

        var gaugeRanges = new[]
        {
            new { Min = 0f, Max = 100f, Unit = "", Label = "" },
            new { Min = 0f, Max = 100f, Unit = "", Label = "" },
            new { Min = 0f, Max = 100f, Unit = "", Label = "" },
            new { Min = 0f, Max = 100f, Unit = "", Label = "" }
        };

        void DrawGauge(Panel gauge, PaintEventArgs e, float value, string title, string unit, float min, float max, Color needleColor)
        {
            var g = e.Graphics;
            g.Clear(gauge.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var cx = gauge.Width / 2f;
            var radius = Math.Min(gauge.Width * 0.38f, gauge.Height * 0.38f);
            var cy = gauge.Height / 2f + radius * 0.35f;
            const float startAngle = 200f;
            const float sweepAngle = 140f;

            using var arcPen = new Pen(Color.FromArgb(80, 130, 210), 8f);
            using var tickPen = new Pen(Color.FromArgb(220, 220, 220), 1.5f);
            using var needlePen = new Pen(needleColor, 3.5f);
            using var centerBrush = new SolidBrush(Color.FromArgb(30, 40, 52));
            using var scaleBrush = new SolidBrush(Color.FromArgb(180, 190, 205));
            using var valueBrush = new SolidBrush(Color.White);
            using var scaleFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var valueFont = new Font("Segoe UI", 16F, FontStyle.Bold);

            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);
            g.DrawArc(arcPen, arcRect, startAngle, sweepAngle);

            for (var i = 0; i <= 10; i++)
            {
                var t = i / 10f;
                var angle = (startAngle + t * sweepAngle) * Math.PI / 180.0;
                var inner = (i % 5 == 0) ? radius - 10f : radius - 6f;
                var outer = radius + 2f;
                var x1 = (float)(cx + inner * Math.Cos(angle));
                var y1 = (float)(cy + inner * Math.Sin(angle));
                var x2 = (float)(cx + outer * Math.Cos(angle));
                var y2 = (float)(cy + outer * Math.Sin(angle));
                g.DrawLine(tickPen, x1, y1, x2, y2);
            }

            var ratio = Math.Clamp((value - min) / (max - min), 0f, 1f);
            var needleAngle = (startAngle + ratio * sweepAngle) * Math.PI / 180.0;
            var needleLength = radius - 12f;
            var needleX = (float)(cx + needleLength * Math.Cos(needleAngle));
            var needleY = (float)(cy + needleLength * Math.Sin(needleAngle));
            g.DrawLine(needlePen, cx, cy, needleX, needleY);

            g.FillEllipse(centerBrush, cx - 12, cy - 12, 24, 24);
            g.DrawEllipse(new Pen(Color.FromArgb(170, 170, 170), 2f), cx - 12, cy - 12, 24, 24);

            using var centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // 0 and 100 scale labels
            g.DrawString("0", scaleFont, scaleBrush, new PointF(cx - radius * 0.82f, cy + 4f), centerFormat);
            g.DrawString("100", scaleFont, scaleBrush, new PointF(cx + radius * 0.82f, cy + 4f), centerFormat);

            // Digital value readout centered below gauge hub
            var valueRect = new RectangleF(0, cy + 17f, gauge.Width, 26f);
            g.DrawString($"{value:0.0}", valueFont, valueBrush, valueRect, centerFormat);
        }

        for (var i = 0; i < gaugePanels.Length; i++)
        {
            var index = i;
            var range = gaugeRanges[i];
            gaugePanels[i].Paint += (s, e) => DrawGauge(gaugePanels[index], e, currentValues[index], range.Label, range.Unit, range.Min, range.Max, index switch
            {
                0 => Color.Orange,
                1 => Color.DeepSkyBlue,
                2 => Color.LimeGreen,
                _ => Color.Magenta
            });
        }

        void DrawTrace(Graphics g, float[] values, Color color, int left, int right, int top, int bottom)
        {
            if (values.Length < 2)
            {
                return;
            }

            using var pen = new Pen(color, 2f);
            var span = Math.Max(1, maxSamples - 1);
            for (var i = 1; i < values.Length; i++)
            {
                var x1 = left + ((i - 1) * (right - left)) / span;
                var y1 = bottom - (Math.Clamp(values[i - 1], 0f, 100f) / 100f) * (bottom - top);
                var x2 = left + (i * (right - left)) / span;
                var y2 = bottom - (Math.Clamp(values[i], 0f, 100f) / 100f) * (bottom - top);
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        trendViewer.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.Clear(trendViewer.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var gridPen = new Pen(Color.FromArgb(55, 80, 110), 1f);
            using var labelBrush = new SolidBrush(Color.White);
            using var font = new Font("Segoe UI", 9F, FontStyle.Bold);

            var chartLeft = 32;
            var chartRight = trendViewer.Width - 20;
            var chartTop = 20;
            var chartBottom = trendViewer.Height - 20;

            for (var i = 0; i <= 6; i++)
            {
                var y = chartTop + i * ((chartBottom - chartTop) / 6);
                g.DrawLine(gridPen, chartLeft, y, chartRight, y);
            }

            for (var i = 0; i <= 8; i++)
            {
                var x = chartLeft + i * ((chartRight - chartLeft) / 8);
                g.DrawLine(gridPen, x, chartTop, x, chartBottom);
            }

            DrawTrace(g, sampleQueues[0].ToArray(), Color.Orange, chartLeft, chartRight, chartTop, chartBottom);
            DrawTrace(g, sampleQueues[1].ToArray(), Color.DeepSkyBlue, chartLeft, chartRight, chartTop, chartBottom);
            DrawTrace(g, sampleQueues[2].ToArray(), Color.LimeGreen, chartLeft, chartRight, chartTop, chartBottom);
            DrawTrace(g, sampleQueues[3].ToArray(), Color.Magenta, chartLeft, chartRight, chartTop, chartBottom);

            g.DrawString("100", font, labelBrush, new PointF(4f, chartTop - 7f));
            g.DrawString("0", font, labelBrush, new PointF(14f, chartBottom - 8f));
            g.DrawString("Last 5 seconds (0.25 Hz, 90° Phase Shift)", font, labelBrush, new PointF(chartLeft, 4f));
        };

        var timer = new System.Windows.Forms.Timer { Interval = 50 };
        timer.Tick += (s, e) =>
        {
            var elapsed = (DateTime.UtcNow - startTime).TotalSeconds;
            var frequency = 0.25f;
            var phases = new[]
            {
                0.0,
                Math.PI / 2.0,
                Math.PI,
                3.0 * Math.PI / 2.0
            };

            var signalBases = new[] { 50f, 50f, 50f, 50f };
            var signalAmplitudes = new[] { 35f, 35f, 35f, 35f };

            for (var i = 0; i < 4; i++)
            {
                var angle = 2.0 * Math.PI * frequency * elapsed + phases[i];
                currentValues[i] = signalBases[i] + signalAmplitudes[i] * (float)Math.Sin(angle);
                currentValues[i] = Math.Clamp(currentValues[i], gaugeRanges[i].Min, gaugeRanges[i].Max);

                sampleQueues[i].Enqueue(currentValues[i]);
                while (sampleQueues[i].Count > maxSamples)
                {
                    sampleQueues[i].Dequeue();
                }

                gaugePanels[i].Invalidate();
            }

            trendViewer.Invalidate();
        };

        tab.Controls.Add(rootLayout);
        timer.Start();
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
