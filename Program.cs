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
        this.Size = new Size(800, 600);
        this.MinimumSize = new Size(600, 400);
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
        var splitContainer = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 250 };

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
