namespace HAL_Display
{
    partial class Display
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.synopticPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabFan = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelFan = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxFanControl = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelFanControl = new System.Windows.Forms.TableLayoutPanel();
            this.labelFanControlFault = new System.Windows.Forms.Label();
            this.textBoxFanControlFault = new System.Windows.Forms.TextBox();
            this.checkBoxFanControlOnOff = new System.Windows.Forms.CheckBox();
            this.buttonFanControlReset = new System.Windows.Forms.Button();
            this.groupBoxFanInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelFanInfoMotorPower = new System.Windows.Forms.Label();
            this.labelFanInfoMotorVoltage = new System.Windows.Forms.Label();
            this.labelFanInfoMotorCurrent = new System.Windows.Forms.Label();
            this.labelFanInfoDCBusV = new System.Windows.Forms.Label();
            this.labelFanInfoHSTemp = new System.Windows.Forms.Label();
            this.labelFanInfoInternalTemp = new System.Windows.Forms.Label();
            this.textBoxFanInfoMotorPower = new System.Windows.Forms.TextBox();
            this.textBoxFanInfoMotorVoltage = new System.Windows.Forms.TextBox();
            this.textBoxFanInfoMotorCurrent = new System.Windows.Forms.TextBox();
            this.textBoxFanInfoDCBusV = new System.Windows.Forms.TextBox();
            this.textBoxFanInfoHSTemp = new System.Windows.Forms.TextBox();
            this.textBoxFanInfoInternalTemp = new System.Windows.Forms.TextBox();
            this.groupBoxFanSpeed = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelFanSpeed = new System.Windows.Forms.TableLayoutPanel();
            this.labelFanSpeedSet = new System.Windows.Forms.Label();
            this.trackBarFanSpeed = new System.Windows.Forms.TrackBar();
            this.textBoxFanSpeedSet = new System.Windows.Forms.TextBox();
            this.textBoxFanSpeedSelected = new System.Windows.Forms.TextBox();
            this.labelFanSpeedTarget = new System.Windows.Forms.Label();
            this.buttonFanSpeedApply = new System.Windows.Forms.Button();
            this.labelFanSpeedCurrent = new System.Windows.Forms.Label();
            this.textBoxFanSpeedCurrent = new System.Windows.Forms.TextBox();
            this.tabOverride = new System.Windows.Forms.TabPage();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.diagnosticsTextBox = new CodeArtEng.Diagnostics.Controls.DiagnosticsTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.tabFan.SuspendLayout();
            this.tableLayoutPanelFan.SuspendLayout();
            this.groupBoxFanControl.SuspendLayout();
            this.tableLayoutPanelFanControl.SuspendLayout();
            this.groupBoxFanInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxFanSpeed.SuspendLayout();
            this.tableLayoutPanelFanSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).BeginInit();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.synopticPanel);
            this.splitContainer1.Panel1MinSize = 400;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(600, 1024);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // synopticPanel
            // 
            this.synopticPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.synopticPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.synopticPanel.Location = new System.Drawing.Point(0, 0);
            this.synopticPanel.MinimumSize = new System.Drawing.Size(600, 400);
            this.synopticPanel.Name = "synopticPanel";
            this.synopticPanel.Size = new System.Drawing.Size(600, 400);
            this.synopticPanel.TabIndex = 0;
            this.synopticPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.synopticPanel_Paint);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStatus);
            this.tabControl1.Controls.Add(this.tabFan);
            this.tabControl1.Controls.Add(this.tabOverride);
            this.tabControl1.Controls.Add(this.tabInfo);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 621);
            this.tabControl1.TabIndex = 0;
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.treeView1);
            this.tabStatus.ImageKey = "thermo.png";
            this.tabStatus.Location = new System.Drawing.Point(4, 31);
            this.tabStatus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabStatus.Size = new System.Drawing.Size(592, 586);
            this.tabStatus.TabIndex = 0;
            this.tabStatus.Text = "Status";
            this.tabStatus.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(2, 3);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(588, 580);
            this.treeView1.TabIndex = 0;
            // 
            // tabFan
            // 
            this.tabFan.AutoScroll = true;
            this.tabFan.Controls.Add(this.tableLayoutPanelFan);
            this.tabFan.ImageKey = "fan2.png";
            this.tabFan.Location = new System.Drawing.Point(4, 33);
            this.tabFan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabFan.Name = "tabFan";
            this.tabFan.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabFan.Size = new System.Drawing.Size(592, 584);
            this.tabFan.TabIndex = 1;
            this.tabFan.Text = " Fan";
            this.tabFan.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelFan
            // 
            this.tableLayoutPanelFan.ColumnCount = 1;
            this.tableLayoutPanelFan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFan.Controls.Add(this.groupBoxFanControl, 0, 2);
            this.tableLayoutPanelFan.Controls.Add(this.groupBoxFanInfo, 0, 0);
            this.tableLayoutPanelFan.Controls.Add(this.groupBoxFanSpeed, 0, 1);
            this.tableLayoutPanelFan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFan.Location = new System.Drawing.Point(2, 3);
            this.tableLayoutPanelFan.Name = "tableLayoutPanelFan";
            this.tableLayoutPanelFan.RowCount = 3;
            this.tableLayoutPanelFan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelFan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelFan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelFan.Size = new System.Drawing.Size(588, 578);
            this.tableLayoutPanelFan.TabIndex = 8;
            // 
            // groupBoxFanControl
            // 
            this.groupBoxFanControl.Controls.Add(this.tableLayoutPanelFanControl);
            this.groupBoxFanControl.Location = new System.Drawing.Point(3, 387);
            this.groupBoxFanControl.Name = "groupBoxFanControl";
            this.groupBoxFanControl.Size = new System.Drawing.Size(582, 160);
            this.groupBoxFanControl.TabIndex = 7;
            this.groupBoxFanControl.TabStop = false;
            this.groupBoxFanControl.Text = "Fan Control";
            // 
            // tableLayoutPanelFanControl
            // 
            this.tableLayoutPanelFanControl.ColumnCount = 2;
            this.tableLayoutPanelFanControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFanControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelFanControl.Controls.Add(this.labelFanControlFault, 1, 0);
            this.tableLayoutPanelFanControl.Controls.Add(this.textBoxFanControlFault, 1, 1);
            this.tableLayoutPanelFanControl.Controls.Add(this.checkBoxFanControlOnOff, 0, 0);
            this.tableLayoutPanelFanControl.Controls.Add(this.buttonFanControlReset, 1, 2);
            this.tableLayoutPanelFanControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFanControl.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanelFanControl.Name = "tableLayoutPanelFanControl";
            this.tableLayoutPanelFanControl.RowCount = 3;
            this.tableLayoutPanelFanControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanelFanControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanelFanControl.Size = new System.Drawing.Size(576, 132);
            this.tableLayoutPanelFanControl.TabIndex = 1;
            // 
            // labelFanControlFault
            // 
            this.labelFanControlFault.AutoSize = true;
            this.labelFanControlFault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanControlFault.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanControlFault.Location = new System.Drawing.Point(291, 0);
            this.labelFanControlFault.Name = "labelFanControlFault";
            this.labelFanControlFault.Size = new System.Drawing.Size(282, 33);
            this.labelFanControlFault.TabIndex = 0;
            this.labelFanControlFault.Text = "Fault";
            this.labelFanControlFault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxFanControlFault
            // 
            this.textBoxFanControlFault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanControlFault.Location = new System.Drawing.Point(291, 43);
            this.textBoxFanControlFault.Name = "textBoxFanControlFault";
            this.textBoxFanControlFault.Size = new System.Drawing.Size(282, 29);
            this.textBoxFanControlFault.TabIndex = 1;
            this.textBoxFanControlFault.Text = "N/A";
            this.textBoxFanControlFault.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxFanControlOnOff
            // 
            this.checkBoxFanControlOnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxFanControlOnOff.AutoSize = true;
            this.checkBoxFanControlOnOff.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxFanControlOnOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxFanControlOnOff.Location = new System.Drawing.Point(3, 3);
            this.checkBoxFanControlOnOff.Name = "checkBoxFanControlOnOff";
            this.tableLayoutPanelFanControl.SetRowSpan(this.checkBoxFanControlOnOff, 3);
            this.checkBoxFanControlOnOff.Size = new System.Drawing.Size(282, 126);
            this.checkBoxFanControlOnOff.TabIndex = 0;
            this.checkBoxFanControlOnOff.Text = "On / Off";
            this.checkBoxFanControlOnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxFanControlOnOff.UseVisualStyleBackColor = false;
            this.checkBoxFanControlOnOff.CheckedChanged += new System.EventHandler(this.checkBoxFanControlOnOff_CheckedChanged);
            // 
            // buttonFanControlReset
            // 
            this.buttonFanControlReset.BackColor = System.Drawing.Color.Transparent;
            this.buttonFanControlReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFanControlReset.Enabled = false;
            this.buttonFanControlReset.Location = new System.Drawing.Point(291, 85);
            this.buttonFanControlReset.Name = "buttonFanControlReset";
            this.buttonFanControlReset.Size = new System.Drawing.Size(282, 44);
            this.buttonFanControlReset.TabIndex = 2;
            this.buttonFanControlReset.Text = "Reset";
            this.buttonFanControlReset.UseVisualStyleBackColor = false;
            // 
            // groupBoxFanInfo
            // 
            this.groupBoxFanInfo.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxFanInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFanInfo.Name = "groupBoxFanInfo";
            this.groupBoxFanInfo.Size = new System.Drawing.Size(582, 160);
            this.groupBoxFanInfo.TabIndex = 8;
            this.groupBoxFanInfo.TabStop = false;
            this.groupBoxFanInfo.Text = "Fan Info";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoMotorPower, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoMotorVoltage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoMotorCurrent, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoDCBusV, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoHSTemp, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelFanInfoInternalTemp, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoMotorPower, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoMotorVoltage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoMotorCurrent, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoDCBusV, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoHSTemp, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFanInfoInternalTemp, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(576, 132);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelFanInfoMotorPower
            // 
            this.labelFanInfoMotorPower.AutoSize = true;
            this.labelFanInfoMotorPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoMotorPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoMotorPower.Location = new System.Drawing.Point(3, 0);
            this.labelFanInfoMotorPower.Name = "labelFanInfoMotorPower";
            this.labelFanInfoMotorPower.Size = new System.Drawing.Size(185, 26);
            this.labelFanInfoMotorPower.TabIndex = 0;
            this.labelFanInfoMotorPower.Text = "Motor Power";
            this.labelFanInfoMotorPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFanInfoMotorVoltage
            // 
            this.labelFanInfoMotorVoltage.AutoSize = true;
            this.labelFanInfoMotorVoltage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoMotorVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoMotorVoltage.Location = new System.Drawing.Point(194, 0);
            this.labelFanInfoMotorVoltage.Name = "labelFanInfoMotorVoltage";
            this.labelFanInfoMotorVoltage.Size = new System.Drawing.Size(186, 26);
            this.labelFanInfoMotorVoltage.TabIndex = 1;
            this.labelFanInfoMotorVoltage.Text = "Motor Voltage";
            this.labelFanInfoMotorVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFanInfoMotorCurrent
            // 
            this.labelFanInfoMotorCurrent.AutoSize = true;
            this.labelFanInfoMotorCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoMotorCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoMotorCurrent.Location = new System.Drawing.Point(386, 0);
            this.labelFanInfoMotorCurrent.Name = "labelFanInfoMotorCurrent";
            this.labelFanInfoMotorCurrent.Size = new System.Drawing.Size(187, 26);
            this.labelFanInfoMotorCurrent.TabIndex = 2;
            this.labelFanInfoMotorCurrent.Text = "Motor Current";
            this.labelFanInfoMotorCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFanInfoDCBusV
            // 
            this.labelFanInfoDCBusV.AutoSize = true;
            this.labelFanInfoDCBusV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoDCBusV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoDCBusV.Location = new System.Drawing.Point(3, 65);
            this.labelFanInfoDCBusV.Name = "labelFanInfoDCBusV";
            this.labelFanInfoDCBusV.Size = new System.Drawing.Size(185, 26);
            this.labelFanInfoDCBusV.TabIndex = 3;
            this.labelFanInfoDCBusV.Text = "Drive DC Bus V";
            this.labelFanInfoDCBusV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFanInfoHSTemp
            // 
            this.labelFanInfoHSTemp.AutoSize = true;
            this.labelFanInfoHSTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoHSTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoHSTemp.Location = new System.Drawing.Point(194, 65);
            this.labelFanInfoHSTemp.Name = "labelFanInfoHSTemp";
            this.labelFanInfoHSTemp.Size = new System.Drawing.Size(186, 26);
            this.labelFanInfoHSTemp.TabIndex = 4;
            this.labelFanInfoHSTemp.Text = "Drive Heatsink Temp";
            this.labelFanInfoHSTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFanInfoInternalTemp
            // 
            this.labelFanInfoInternalTemp.AutoSize = true;
            this.labelFanInfoInternalTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanInfoInternalTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanInfoInternalTemp.Location = new System.Drawing.Point(386, 65);
            this.labelFanInfoInternalTemp.Name = "labelFanInfoInternalTemp";
            this.labelFanInfoInternalTemp.Size = new System.Drawing.Size(187, 26);
            this.labelFanInfoInternalTemp.TabIndex = 5;
            this.labelFanInfoInternalTemp.Text = "Drive Internal Temp";
            this.labelFanInfoInternalTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxFanInfoMotorPower
            // 
            this.textBoxFanInfoMotorPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoMotorPower.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoMotorPower.Location = new System.Drawing.Point(3, 31);
            this.textBoxFanInfoMotorPower.Name = "textBoxFanInfoMotorPower";
            this.textBoxFanInfoMotorPower.ReadOnly = true;
            this.textBoxFanInfoMotorPower.Size = new System.Drawing.Size(185, 29);
            this.textBoxFanInfoMotorPower.TabIndex = 6;
            this.textBoxFanInfoMotorPower.Text = "N/A";
            this.textBoxFanInfoMotorPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanInfoMotorVoltage
            // 
            this.textBoxFanInfoMotorVoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoMotorVoltage.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoMotorVoltage.Location = new System.Drawing.Point(194, 31);
            this.textBoxFanInfoMotorVoltage.Name = "textBoxFanInfoMotorVoltage";
            this.textBoxFanInfoMotorVoltage.ReadOnly = true;
            this.textBoxFanInfoMotorVoltage.Size = new System.Drawing.Size(186, 29);
            this.textBoxFanInfoMotorVoltage.TabIndex = 7;
            this.textBoxFanInfoMotorVoltage.Text = "N/A";
            this.textBoxFanInfoMotorVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanInfoMotorCurrent
            // 
            this.textBoxFanInfoMotorCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoMotorCurrent.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoMotorCurrent.Location = new System.Drawing.Point(386, 31);
            this.textBoxFanInfoMotorCurrent.Name = "textBoxFanInfoMotorCurrent";
            this.textBoxFanInfoMotorCurrent.ReadOnly = true;
            this.textBoxFanInfoMotorCurrent.Size = new System.Drawing.Size(187, 29);
            this.textBoxFanInfoMotorCurrent.TabIndex = 8;
            this.textBoxFanInfoMotorCurrent.Text = "N/A";
            this.textBoxFanInfoMotorCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanInfoDCBusV
            // 
            this.textBoxFanInfoDCBusV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoDCBusV.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoDCBusV.Location = new System.Drawing.Point(3, 97);
            this.textBoxFanInfoDCBusV.Name = "textBoxFanInfoDCBusV";
            this.textBoxFanInfoDCBusV.ReadOnly = true;
            this.textBoxFanInfoDCBusV.Size = new System.Drawing.Size(185, 29);
            this.textBoxFanInfoDCBusV.TabIndex = 9;
            this.textBoxFanInfoDCBusV.Text = "N/A";
            this.textBoxFanInfoDCBusV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanInfoHSTemp
            // 
            this.textBoxFanInfoHSTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoHSTemp.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoHSTemp.Location = new System.Drawing.Point(194, 97);
            this.textBoxFanInfoHSTemp.Name = "textBoxFanInfoHSTemp";
            this.textBoxFanInfoHSTemp.ReadOnly = true;
            this.textBoxFanInfoHSTemp.Size = new System.Drawing.Size(186, 29);
            this.textBoxFanInfoHSTemp.TabIndex = 10;
            this.textBoxFanInfoHSTemp.Text = "N/A";
            this.textBoxFanInfoHSTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanInfoInternalTemp
            // 
            this.textBoxFanInfoInternalTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanInfoInternalTemp.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanInfoInternalTemp.Location = new System.Drawing.Point(386, 97);
            this.textBoxFanInfoInternalTemp.Name = "textBoxFanInfoInternalTemp";
            this.textBoxFanInfoInternalTemp.ReadOnly = true;
            this.textBoxFanInfoInternalTemp.Size = new System.Drawing.Size(187, 29);
            this.textBoxFanInfoInternalTemp.TabIndex = 11;
            this.textBoxFanInfoInternalTemp.Text = "N/A";
            this.textBoxFanInfoInternalTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxFanSpeed
            // 
            this.groupBoxFanSpeed.Controls.Add(this.tableLayoutPanelFanSpeed);
            this.groupBoxFanSpeed.Location = new System.Drawing.Point(3, 195);
            this.groupBoxFanSpeed.Name = "groupBoxFanSpeed";
            this.groupBoxFanSpeed.Size = new System.Drawing.Size(582, 160);
            this.groupBoxFanSpeed.TabIndex = 6;
            this.groupBoxFanSpeed.TabStop = false;
            this.groupBoxFanSpeed.Text = "Fan Speed";
            // 
            // tableLayoutPanelFanSpeed
            // 
            this.tableLayoutPanelFanSpeed.ColumnCount = 4;
            this.tableLayoutPanelFanSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFanSpeed.Controls.Add(this.labelFanSpeedSet, 2, 1);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.trackBarFanSpeed, 0, 0);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.textBoxFanSpeedSet, 2, 2);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.textBoxFanSpeedSelected, 0, 2);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.labelFanSpeedTarget, 0, 1);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.buttonFanSpeedApply, 1, 1);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.labelFanSpeedCurrent, 3, 1);
            this.tableLayoutPanelFanSpeed.Controls.Add(this.textBoxFanSpeedCurrent, 3, 2);
            this.tableLayoutPanelFanSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFanSpeed.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanelFanSpeed.Name = "tableLayoutPanelFanSpeed";
            this.tableLayoutPanelFanSpeed.RowCount = 3;
            this.tableLayoutPanelFanSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanelFanSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelFanSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanelFanSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFanSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelFanSpeed.Size = new System.Drawing.Size(576, 132);
            this.tableLayoutPanelFanSpeed.TabIndex = 6;
            // 
            // labelFanSpeedSet
            // 
            this.labelFanSpeedSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFanSpeedSet.AutoSize = true;
            this.labelFanSpeedSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanSpeedSet.Location = new System.Drawing.Point(291, 49);
            this.labelFanSpeedSet.Name = "labelFanSpeedSet";
            this.labelFanSpeedSet.Size = new System.Drawing.Size(138, 33);
            this.labelFanSpeedSet.TabIndex = 4;
            this.labelFanSpeedSet.Text = "Set Point";
            this.labelFanSpeedSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarFanSpeed
            // 
            this.trackBarFanSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarFanSpeed.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelFanSpeed.SetColumnSpan(this.trackBarFanSpeed, 4);
            this.trackBarFanSpeed.Location = new System.Drawing.Point(0, 0);
            this.trackBarFanSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarFanSpeed.Maximum = 60;
            this.trackBarFanSpeed.Name = "trackBarFanSpeed";
            this.trackBarFanSpeed.Size = new System.Drawing.Size(576, 49);
            this.trackBarFanSpeed.TabIndex = 1;
            this.trackBarFanSpeed.Value = 30;
            this.trackBarFanSpeed.ValueChanged += new System.EventHandler(this.trackBarFanSpeed_ValueChanged);
            // 
            // textBoxFanSpeedSet
            // 
            this.textBoxFanSpeedSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanSpeedSet.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFanSpeedSet.Location = new System.Drawing.Point(291, 92);
            this.textBoxFanSpeedSet.Name = "textBoxFanSpeedSet";
            this.textBoxFanSpeedSet.ReadOnly = true;
            this.textBoxFanSpeedSet.Size = new System.Drawing.Size(138, 29);
            this.textBoxFanSpeedSet.TabIndex = 5;
            this.textBoxFanSpeedSet.Text = "N/A";
            this.textBoxFanSpeedSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxFanSpeedSelected
            // 
            this.textBoxFanSpeedSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanSpeedSelected.Location = new System.Drawing.Point(3, 92);
            this.textBoxFanSpeedSelected.Name = "textBoxFanSpeedSelected";
            this.textBoxFanSpeedSelected.Size = new System.Drawing.Size(138, 29);
            this.textBoxFanSpeedSelected.TabIndex = 5;
            this.textBoxFanSpeedSelected.Text = "N/A";
            this.textBoxFanSpeedSelected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelFanSpeedTarget
            // 
            this.labelFanSpeedTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFanSpeedTarget.AutoSize = true;
            this.labelFanSpeedTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanSpeedTarget.Location = new System.Drawing.Point(3, 49);
            this.labelFanSpeedTarget.Name = "labelFanSpeedTarget";
            this.labelFanSpeedTarget.Size = new System.Drawing.Size(138, 33);
            this.labelFanSpeedTarget.TabIndex = 3;
            this.labelFanSpeedTarget.Text = "Selected Speed";
            this.labelFanSpeedTarget.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonFanSpeedApply
            // 
            this.buttonFanSpeedApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFanSpeedApply.Enabled = false;
            this.buttonFanSpeedApply.Location = new System.Drawing.Point(147, 52);
            this.buttonFanSpeedApply.Name = "buttonFanSpeedApply";
            this.tableLayoutPanelFanSpeed.SetRowSpan(this.buttonFanSpeedApply, 2);
            this.buttonFanSpeedApply.Size = new System.Drawing.Size(138, 77);
            this.buttonFanSpeedApply.TabIndex = 6;
            this.buttonFanSpeedApply.Text = "Apply";
            this.buttonFanSpeedApply.UseVisualStyleBackColor = true;
            this.buttonFanSpeedApply.Click += new System.EventHandler(this.buttonFanSpeedApply_Click);
            // 
            // labelFanSpeedCurrent
            // 
            this.labelFanSpeedCurrent.AutoSize = true;
            this.labelFanSpeedCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFanSpeedCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanSpeedCurrent.Location = new System.Drawing.Point(435, 49);
            this.labelFanSpeedCurrent.Name = "labelFanSpeedCurrent";
            this.labelFanSpeedCurrent.Size = new System.Drawing.Size(138, 33);
            this.labelFanSpeedCurrent.TabIndex = 7;
            this.labelFanSpeedCurrent.Text = "Current Speed";
            this.labelFanSpeedCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxFanSpeedCurrent
            // 
            this.textBoxFanSpeedCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFanSpeedCurrent.Location = new System.Drawing.Point(435, 92);
            this.textBoxFanSpeedCurrent.Name = "textBoxFanSpeedCurrent";
            this.textBoxFanSpeedCurrent.Size = new System.Drawing.Size(138, 29);
            this.textBoxFanSpeedCurrent.TabIndex = 8;
            this.textBoxFanSpeedCurrent.Text = "N/A";
            this.textBoxFanSpeedCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabOverride
            // 
            this.tabOverride.ImageKey = "manual.png";
            this.tabOverride.Location = new System.Drawing.Point(4, 31);
            this.tabOverride.Name = "tabOverride";
            this.tabOverride.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverride.Size = new System.Drawing.Size(592, 586);
            this.tabOverride.TabIndex = 2;
            this.tabOverride.Text = "Manual Override";
            this.tabOverride.UseVisualStyleBackColor = true;
            // 
            // tabInfo
            // 
            this.tabInfo.ImageKey = "tree.png";
            this.tabInfo.Location = new System.Drawing.Point(4, 31);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Size = new System.Drawing.Size(592, 586);
            this.tabInfo.TabIndex = 3;
            this.tabInfo.Text = "System";
            this.tabInfo.UseVisualStyleBackColor = true;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.diagnosticsTextBox);
            this.tabLog.ImageKey = "log.png";
            this.tabLog.Location = new System.Drawing.Point(4, 31);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(592, 586);
            this.tabLog.TabIndex = 4;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // diagnosticsTextBox
            // 
            this.diagnosticsTextBox.AcceptsReturn = true;
            this.diagnosticsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.diagnosticsTextBox.DisplayBufferSize = 1000;
            this.diagnosticsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticsTextBox.FlushEnabled = false;
            this.diagnosticsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.diagnosticsTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.diagnosticsTextBox.Location = new System.Drawing.Point(3, 3);
            this.diagnosticsTextBox.Multiline = true;
            this.diagnosticsTextBox.Name = "diagnosticsTextBox";
            this.diagnosticsTextBox.OutputFile = null;
            this.diagnosticsTextBox.OutputFileBackup = null;
            this.diagnosticsTextBox.ReadOnly = true;
            this.diagnosticsTextBox.RefreshInterval = 50;
            this.diagnosticsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.diagnosticsTextBox.Size = new System.Drawing.Size(586, 580);
            this.diagnosticsTextBox.TabIndex = 0;
            this.diagnosticsTextBox.TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff";
            this.diagnosticsTextBox.WordWrap = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "fan2.png");
            this.imageList1.Images.SetKeyName(1, "thermo.png");
            this.imageList1.Images.SetKeyName(2, "tree.png");
            this.imageList1.Images.SetKeyName(3, "log.png");
            this.imageList1.Images.SetKeyName(4, "manual.png");
            this.imageList1.Images.SetKeyName(5, "fan3.png");
            // 
            // Display
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(600, 1024);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "Display";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "HAL Display";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.display_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.tabFan.ResumeLayout(false);
            this.tableLayoutPanelFan.ResumeLayout(false);
            this.groupBoxFanControl.ResumeLayout(false);
            this.tableLayoutPanelFanControl.ResumeLayout(false);
            this.tableLayoutPanelFanControl.PerformLayout();
            this.groupBoxFanInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxFanSpeed.ResumeLayout(false);
            this.tableLayoutPanelFanSpeed.ResumeLayout(false);
            this.tableLayoutPanelFanSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage tabFan;
        private System.Windows.Forms.Panel synopticPanel;
        private System.Windows.Forms.TabPage tabOverride;
        public System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabLog;
        private CodeArtEng.Diagnostics.Controls.DiagnosticsTextBox diagnosticsTextBox;
        private System.Windows.Forms.GroupBox groupBoxFanControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFanControl;
        private System.Windows.Forms.CheckBox checkBoxFanControlOnOff;
        private System.Windows.Forms.GroupBox groupBoxFanSpeed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFanSpeed;
        private System.Windows.Forms.Label labelFanSpeedSet;
        private System.Windows.Forms.TrackBar trackBarFanSpeed;
        private System.Windows.Forms.Label labelFanSpeedTarget;
        private System.Windows.Forms.Button buttonFanSpeedApply;
        private System.Windows.Forms.Label labelFanControlFault;
        private System.Windows.Forms.TextBox textBoxFanControlFault;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFan;
        private System.Windows.Forms.Button buttonFanControlReset;
        private System.Windows.Forms.GroupBox groupBoxFanInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelFanInfoMotorPower;
        private System.Windows.Forms.Label labelFanInfoMotorVoltage;
        private System.Windows.Forms.Label labelFanInfoMotorCurrent;
        private System.Windows.Forms.Label labelFanInfoDCBusV;
        private System.Windows.Forms.Label labelFanInfoHSTemp;
        private System.Windows.Forms.Label labelFanInfoInternalTemp;
        private System.Windows.Forms.TextBox textBoxFanInfoMotorPower;
        private System.Windows.Forms.TextBox textBoxFanInfoMotorVoltage;
        private System.Windows.Forms.TextBox textBoxFanInfoMotorCurrent;
        private System.Windows.Forms.TextBox textBoxFanInfoDCBusV;
        private System.Windows.Forms.TextBox textBoxFanInfoHSTemp;
        private System.Windows.Forms.TextBox textBoxFanInfoInternalTemp;
        private System.Windows.Forms.TextBox textBoxFanSpeedSet;
        private System.Windows.Forms.TextBox textBoxFanSpeedSelected;
        private System.Windows.Forms.Label labelFanSpeedCurrent;
        private System.Windows.Forms.TextBox textBoxFanSpeedCurrent;
    }
}

