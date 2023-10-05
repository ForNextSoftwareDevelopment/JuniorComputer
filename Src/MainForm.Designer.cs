
namespace JuniorComputer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetRAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disAssemblerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxFlags = new System.Windows.Forms.GroupBox();
            this.chkFlagI = new System.Windows.Forms.CheckBox();
            this.lblFlagI = new System.Windows.Forms.Label();
            this.chkFlag1 = new System.Windows.Forms.CheckBox();
            this.lblFlag1 = new System.Windows.Forms.Label();
            this.chkFlagV = new System.Windows.Forms.CheckBox();
            this.lblFlagV = new System.Windows.Forms.Label();
            this.chkFlagC = new System.Windows.Forms.CheckBox();
            this.chkFlagB = new System.Windows.Forms.CheckBox();
            this.chkFlagD = new System.Windows.Forms.CheckBox();
            this.chkFlagZ = new System.Windows.Forms.CheckBox();
            this.chkFlagN = new System.Windows.Forms.CheckBox();
            this.lblFlagC = new System.Windows.Forms.Label();
            this.lblFlagB = new System.Windows.Forms.Label();
            this.lblFlagD = new System.Windows.Forms.Label();
            this.lblFlagZ = new System.Windows.Forms.Label();
            this.lblFlagN = new System.Windows.Forms.Label();
            this.groupBoxRegisters = new System.Windows.Forms.GroupBox();
            this.labelSPRegister = new System.Windows.Forms.Label();
            this.labelPCRegister = new System.Windows.Forms.Label();
            this.labelYRegister = new System.Windows.Forms.Label();
            this.labelXRegister = new System.Windows.Forms.Label();
            this.labelARegister = new System.Windows.Forms.Label();
            this.lblSP = new System.Windows.Forms.Label();
            this.lblPC = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.panelMemoryInfo = new System.Windows.Forms.Panel();
            this.lblValueMemory = new System.Windows.Forms.Label();
            this.lblMemoryAddress = new System.Windows.Forms.Label();
            this.btnMemoryWrite = new System.Windows.Forms.Button();
            this.numMemoryAddress = new System.Windows.Forms.NumericUpDown();
            this.tbMemoryUpdateByte = new System.Windows.Forms.TextBox();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnMemoryStartAddress = new System.Windows.Forms.Button();
            this.tbMemoryStartAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRestartSimulator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStartDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFast = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStep = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolTipRegisterBinary = new System.Windows.Forms.ToolTip(this.components);
            this.lblLine = new System.Windows.Forms.Label();
            this.lblColumn = new System.Windows.Forms.Label();
            this.btnViewProgram = new System.Windows.Forms.Button();
            this.btnViewSymbolTable = new System.Windows.Forms.Button();
            this.panelMemory = new System.Windows.Forms.Panel();
            this.chkLatched = new System.Windows.Forms.CheckBox();
            this.btnViewSP = new System.Windows.Forms.Button();
            this.btnViewPC = new System.Windows.Forms.Button();
            this.chkLock = new System.Windows.Forms.CheckBox();
            this.panelWriteMemory = new System.Windows.Forms.Panel();
            this.richTextBoxProgram = new System.Windows.Forms.RichTextBox();
            this.btnClearBreakPoint = new System.Windows.Forms.Button();
            this.pbBreakPoint = new System.Windows.Forms.PictureBox();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblSetProgramCounter = new System.Windows.Forms.Label();
            this.tbSetProgramCounter = new System.Windows.Forms.TextBox();
            this.panelInstructions = new System.Windows.Forms.Panel();
            this.ucJunior = new JuniorComputer.UCJunior();
            this.chkInsertMonitor = new System.Windows.Forms.CheckBox();
            this.menuStrip.SuspendLayout();
            this.groupBoxFlags.SuspendLayout();
            this.groupBoxRegisters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMemoryAddress)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.panelMemory.SuspendLayout();
            this.panelWriteMemory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBreakPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.disAssemblerToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(9, 8);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(227, 24);
            this.menuStrip.TabIndex = 13;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveBinaryToolStripMenuItem,
            this.toolStripMenuItem3,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newFileToolStripMenuItem.Image")));
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.newFileToolStripMenuItem.Text = "&New";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.new_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.open_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.save_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.save_as;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAs_Click);
            // 
            // saveBinaryToolStripMenuItem
            // 
            this.saveBinaryToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.save_binary;
            this.saveBinaryToolStripMenuItem.Name = "saveBinaryToolStripMenuItem";
            this.saveBinaryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.saveBinaryToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveBinaryToolStripMenuItem.Text = "Save &Binary";
            this.saveBinaryToolStripMenuItem.Click += new System.EventHandler(this.saveBinary_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(204, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quit_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetRAMToolStripMenuItem,
            this.toolStripMenuItem1,
            this.resetSimulatorToolStripMenuItem});
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.resetToolStripMenuItem.Text = "&Reset";
            // 
            // resetRAMToolStripMenuItem
            // 
            this.resetRAMToolStripMenuItem.Name = "resetRAMToolStripMenuItem";
            this.resetRAMToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.resetRAMToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.resetRAMToolStripMenuItem.Text = "Reset RAM";
            this.resetRAMToolStripMenuItem.Click += new System.EventHandler(this.resetRAM_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(201, 6);
            // 
            // resetSimulatorToolStripMenuItem
            // 
            this.resetSimulatorToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.reset;
            this.resetSimulatorToolStripMenuItem.Name = "resetSimulatorToolStripMenuItem";
            this.resetSimulatorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.resetSimulatorToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.resetSimulatorToolStripMenuItem.Text = "&Reset Simulator";
            this.resetSimulatorToolStripMenuItem.Click += new System.EventHandler(this.resetSimulator_Click);
            // 
            // disAssemblerToolStripMenuItem
            // 
            this.disAssemblerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBinaryToolStripMenuItem});
            this.disAssemblerToolStripMenuItem.Name = "disAssemblerToolStripMenuItem";
            this.disAssemblerToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.disAssemblerToolStripMenuItem.Text = "DisAssembler";
            // 
            // openBinaryToolStripMenuItem
            // 
            this.openBinaryToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.open;
            this.openBinaryToolStripMenuItem.Name = "openBinaryToolStripMenuItem";
            this.openBinaryToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openBinaryToolStripMenuItem.Text = "Open Binary";
            this.openBinaryToolStripMenuItem.Click += new System.EventHandler(this.openBinary_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.Image = global::JuniorComputer.Properties.Resources.help;
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.viewHelpToolStripMenuItem.Text = "View Help";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelp_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.about_Click);
            // 
            // groupBoxFlags
            // 
            this.groupBoxFlags.BackColor = System.Drawing.SystemColors.Info;
            this.groupBoxFlags.Controls.Add(this.chkFlagI);
            this.groupBoxFlags.Controls.Add(this.lblFlagI);
            this.groupBoxFlags.Controls.Add(this.chkFlag1);
            this.groupBoxFlags.Controls.Add(this.lblFlag1);
            this.groupBoxFlags.Controls.Add(this.chkFlagV);
            this.groupBoxFlags.Controls.Add(this.lblFlagV);
            this.groupBoxFlags.Controls.Add(this.chkFlagC);
            this.groupBoxFlags.Controls.Add(this.chkFlagB);
            this.groupBoxFlags.Controls.Add(this.chkFlagD);
            this.groupBoxFlags.Controls.Add(this.chkFlagZ);
            this.groupBoxFlags.Controls.Add(this.chkFlagN);
            this.groupBoxFlags.Controls.Add(this.lblFlagC);
            this.groupBoxFlags.Controls.Add(this.lblFlagB);
            this.groupBoxFlags.Controls.Add(this.lblFlagD);
            this.groupBoxFlags.Controls.Add(this.lblFlagZ);
            this.groupBoxFlags.Controls.Add(this.lblFlagN);
            this.groupBoxFlags.Location = new System.Drawing.Point(13, 127);
            this.groupBoxFlags.Name = "groupBoxFlags";
            this.groupBoxFlags.Size = new System.Drawing.Size(289, 53);
            this.groupBoxFlags.TabIndex = 3;
            this.groupBoxFlags.TabStop = false;
            this.groupBoxFlags.Text = "Flags";
            // 
            // chkFlagI
            // 
            this.chkFlagI.AutoSize = true;
            this.chkFlagI.Location = new System.Drawing.Point(192, 33);
            this.chkFlagI.Name = "chkFlagI";
            this.chkFlagI.Size = new System.Drawing.Size(15, 14);
            this.chkFlagI.TabIndex = 24;
            this.chkFlagI.UseVisualStyleBackColor = true;
            this.chkFlagI.CheckedChanged += new System.EventHandler(this.chkFlagI_CheckedChanged);
            // 
            // lblFlagI
            // 
            this.lblFlagI.AutoSize = true;
            this.lblFlagI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagI.Location = new System.Drawing.Point(192, 17);
            this.lblFlagI.Name = "lblFlagI";
            this.lblFlagI.Size = new System.Drawing.Size(11, 13);
            this.lblFlagI.TabIndex = 23;
            this.lblFlagI.Text = "I";
            // 
            // chkFlag1
            // 
            this.chkFlag1.AutoSize = true;
            this.chkFlag1.Checked = true;
            this.chkFlag1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFlag1.Enabled = false;
            this.chkFlag1.Location = new System.Drawing.Point(84, 33);
            this.chkFlag1.Name = "chkFlag1";
            this.chkFlag1.Size = new System.Drawing.Size(15, 14);
            this.chkFlag1.TabIndex = 22;
            this.chkFlag1.UseVisualStyleBackColor = true;
            // 
            // lblFlag1
            // 
            this.lblFlag1.AutoSize = true;
            this.lblFlag1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlag1.Location = new System.Drawing.Point(84, 17);
            this.lblFlag1.Name = "lblFlag1";
            this.lblFlag1.Size = new System.Drawing.Size(14, 13);
            this.lblFlag1.TabIndex = 21;
            this.lblFlag1.Text = "1";
            // 
            // chkFlagV
            // 
            this.chkFlagV.AutoSize = true;
            this.chkFlagV.Location = new System.Drawing.Point(49, 33);
            this.chkFlagV.Name = "chkFlagV";
            this.chkFlagV.Size = new System.Drawing.Size(15, 14);
            this.chkFlagV.TabIndex = 20;
            this.chkFlagV.UseVisualStyleBackColor = true;
            this.chkFlagV.CheckedChanged += new System.EventHandler(this.chkFlagV_CheckedChanged);
            // 
            // lblFlagV
            // 
            this.lblFlagV.AutoSize = true;
            this.lblFlagV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagV.Location = new System.Drawing.Point(49, 17);
            this.lblFlagV.Name = "lblFlagV";
            this.lblFlagV.Size = new System.Drawing.Size(15, 13);
            this.lblFlagV.TabIndex = 19;
            this.lblFlagV.Text = "V";
            // 
            // chkFlagC
            // 
            this.chkFlagC.AutoSize = true;
            this.chkFlagC.Location = new System.Drawing.Point(257, 33);
            this.chkFlagC.Name = "chkFlagC";
            this.chkFlagC.Size = new System.Drawing.Size(15, 14);
            this.chkFlagC.TabIndex = 18;
            this.chkFlagC.UseVisualStyleBackColor = true;
            this.chkFlagC.CheckedChanged += new System.EventHandler(this.chkFlagC_CheckedChanged);
            // 
            // chkFlagB
            // 
            this.chkFlagB.AutoSize = true;
            this.chkFlagB.Location = new System.Drawing.Point(123, 33);
            this.chkFlagB.Name = "chkFlagB";
            this.chkFlagB.Size = new System.Drawing.Size(15, 14);
            this.chkFlagB.TabIndex = 17;
            this.chkFlagB.UseVisualStyleBackColor = true;
            this.chkFlagB.CheckedChanged += new System.EventHandler(this.chkFlagB_CheckedChanged);
            // 
            // chkFlagD
            // 
            this.chkFlagD.AutoSize = true;
            this.chkFlagD.Location = new System.Drawing.Point(157, 33);
            this.chkFlagD.Name = "chkFlagD";
            this.chkFlagD.Size = new System.Drawing.Size(15, 14);
            this.chkFlagD.TabIndex = 16;
            this.chkFlagD.UseVisualStyleBackColor = true;
            this.chkFlagD.CheckedChanged += new System.EventHandler(this.chkFlagD_CheckedChanged);
            // 
            // chkFlagZ
            // 
            this.chkFlagZ.AutoSize = true;
            this.chkFlagZ.Location = new System.Drawing.Point(226, 33);
            this.chkFlagZ.Name = "chkFlagZ";
            this.chkFlagZ.Size = new System.Drawing.Size(15, 14);
            this.chkFlagZ.TabIndex = 15;
            this.chkFlagZ.UseVisualStyleBackColor = true;
            this.chkFlagZ.CheckedChanged += new System.EventHandler(this.chkFlagZ_CheckedChanged);
            // 
            // chkFlagN
            // 
            this.chkFlagN.AutoSize = true;
            this.chkFlagN.Location = new System.Drawing.Point(11, 33);
            this.chkFlagN.Name = "chkFlagN";
            this.chkFlagN.Size = new System.Drawing.Size(15, 14);
            this.chkFlagN.TabIndex = 10;
            this.chkFlagN.UseVisualStyleBackColor = true;
            this.chkFlagN.CheckedChanged += new System.EventHandler(this.chkFlagN_CheckedChanged);
            // 
            // lblFlagC
            // 
            this.lblFlagC.AutoSize = true;
            this.lblFlagC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagC.Location = new System.Drawing.Point(257, 17);
            this.lblFlagC.Name = "lblFlagC";
            this.lblFlagC.Size = new System.Drawing.Size(15, 13);
            this.lblFlagC.TabIndex = 7;
            this.lblFlagC.Text = "C";
            // 
            // lblFlagB
            // 
            this.lblFlagB.AutoSize = true;
            this.lblFlagB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagB.Location = new System.Drawing.Point(123, 17);
            this.lblFlagB.Name = "lblFlagB";
            this.lblFlagB.Size = new System.Drawing.Size(15, 13);
            this.lblFlagB.TabIndex = 5;
            this.lblFlagB.Text = "B";
            // 
            // lblFlagD
            // 
            this.lblFlagD.AutoSize = true;
            this.lblFlagD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagD.Location = new System.Drawing.Point(156, 17);
            this.lblFlagD.Name = "lblFlagD";
            this.lblFlagD.Size = new System.Drawing.Size(16, 13);
            this.lblFlagD.TabIndex = 3;
            this.lblFlagD.Text = "D";
            // 
            // lblFlagZ
            // 
            this.lblFlagZ.AutoSize = true;
            this.lblFlagZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagZ.Location = new System.Drawing.Point(226, 17);
            this.lblFlagZ.Name = "lblFlagZ";
            this.lblFlagZ.Size = new System.Drawing.Size(15, 13);
            this.lblFlagZ.TabIndex = 1;
            this.lblFlagZ.Text = "Z";
            // 
            // lblFlagN
            // 
            this.lblFlagN.AutoSize = true;
            this.lblFlagN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagN.Location = new System.Drawing.Point(10, 17);
            this.lblFlagN.Name = "lblFlagN";
            this.lblFlagN.Size = new System.Drawing.Size(16, 13);
            this.lblFlagN.TabIndex = 0;
            this.lblFlagN.Text = "N";
            // 
            // groupBoxRegisters
            // 
            this.groupBoxRegisters.BackColor = System.Drawing.SystemColors.Info;
            this.groupBoxRegisters.Controls.Add(this.labelSPRegister);
            this.groupBoxRegisters.Controls.Add(this.labelPCRegister);
            this.groupBoxRegisters.Controls.Add(this.labelYRegister);
            this.groupBoxRegisters.Controls.Add(this.labelXRegister);
            this.groupBoxRegisters.Controls.Add(this.labelARegister);
            this.groupBoxRegisters.Controls.Add(this.lblSP);
            this.groupBoxRegisters.Controls.Add(this.lblPC);
            this.groupBoxRegisters.Controls.Add(this.lblY);
            this.groupBoxRegisters.Controls.Add(this.lblX);
            this.groupBoxRegisters.Controls.Add(this.lblA);
            this.groupBoxRegisters.Location = new System.Drawing.Point(12, 40);
            this.groupBoxRegisters.Name = "groupBoxRegisters";
            this.groupBoxRegisters.Size = new System.Drawing.Size(290, 81);
            this.groupBoxRegisters.TabIndex = 2;
            this.groupBoxRegisters.TabStop = false;
            this.groupBoxRegisters.Text = "Registers";
            // 
            // labelSPRegister
            // 
            this.labelSPRegister.AutoSize = true;
            this.labelSPRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSPRegister.Location = new System.Drawing.Point(239, 54);
            this.labelSPRegister.Name = "labelSPRegister";
            this.labelSPRegister.Size = new System.Drawing.Size(45, 20);
            this.labelSPRegister.TabIndex = 27;
            this.labelSPRegister.Text = "0100";
            this.labelSPRegister.MouseHover += new System.EventHandler(this.labelSPRegister_MouseHover);
            // 
            // labelPCRegister
            // 
            this.labelPCRegister.AutoSize = true;
            this.labelPCRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPCRegister.Location = new System.Drawing.Point(239, 15);
            this.labelPCRegister.Name = "labelPCRegister";
            this.labelPCRegister.Size = new System.Drawing.Size(45, 20);
            this.labelPCRegister.TabIndex = 26;
            this.labelPCRegister.Text = "0000";
            this.labelPCRegister.MouseHover += new System.EventHandler(this.labelPCRegister_MouseHover);
            // 
            // labelYRegister
            // 
            this.labelYRegister.AutoSize = true;
            this.labelYRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYRegister.Location = new System.Drawing.Point(55, 54);
            this.labelYRegister.Name = "labelYRegister";
            this.labelYRegister.Size = new System.Drawing.Size(27, 20);
            this.labelYRegister.TabIndex = 21;
            this.labelYRegister.Text = "00";
            this.labelYRegister.MouseHover += new System.EventHandler(this.labelYRegister_MouseHover);
            // 
            // labelXRegister
            // 
            this.labelXRegister.AutoSize = true;
            this.labelXRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXRegister.Location = new System.Drawing.Point(55, 35);
            this.labelXRegister.Name = "labelXRegister";
            this.labelXRegister.Size = new System.Drawing.Size(27, 20);
            this.labelXRegister.TabIndex = 20;
            this.labelXRegister.Text = "00";
            this.labelXRegister.MouseHover += new System.EventHandler(this.labelXRegister_MouseHover);
            // 
            // labelARegister
            // 
            this.labelARegister.AutoSize = true;
            this.labelARegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelARegister.Location = new System.Drawing.Point(55, 16);
            this.labelARegister.Name = "labelARegister";
            this.labelARegister.Size = new System.Drawing.Size(27, 20);
            this.labelARegister.TabIndex = 19;
            this.labelARegister.Text = "00";
            this.labelARegister.MouseHover += new System.EventHandler(this.labelARegister_MouseHover);
            // 
            // lblSP
            // 
            this.lblSP.AutoSize = true;
            this.lblSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSP.Location = new System.Drawing.Point(199, 54);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(32, 20);
            this.lblSP.TabIndex = 18;
            this.lblSP.Text = "SP";
            // 
            // lblPC
            // 
            this.lblPC.AutoSize = true;
            this.lblPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPC.Location = new System.Drawing.Point(199, 16);
            this.lblPC.Name = "lblPC";
            this.lblPC.Size = new System.Drawing.Size(32, 20);
            this.lblPC.TabIndex = 17;
            this.lblPC.Text = "PC";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY.Location = new System.Drawing.Point(6, 54);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(21, 20);
            this.lblY.TabIndex = 12;
            this.lblY.Text = "Y";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblX.Location = new System.Drawing.Point(6, 35);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(21, 20);
            this.lblX.TabIndex = 11;
            this.lblX.Text = "X";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblA.Location = new System.Drawing.Point(6, 16);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(21, 20);
            this.lblA.TabIndex = 10;
            this.lblA.Text = "A";
            // 
            // panelMemoryInfo
            // 
            this.panelMemoryInfo.BackColor = System.Drawing.SystemColors.Info;
            this.panelMemoryInfo.Location = new System.Drawing.Point(6, 32);
            this.panelMemoryInfo.Name = "panelMemoryInfo";
            this.panelMemoryInfo.Size = new System.Drawing.Size(534, 348);
            this.panelMemoryInfo.TabIndex = 6;
            // 
            // lblValueMemory
            // 
            this.lblValueMemory.AutoSize = true;
            this.lblValueMemory.Location = new System.Drawing.Point(76, 4);
            this.lblValueMemory.Name = "lblValueMemory";
            this.lblValueMemory.Size = new System.Drawing.Size(34, 13);
            this.lblValueMemory.TabIndex = 10;
            this.lblValueMemory.Text = "Value";
            // 
            // lblMemoryAddress
            // 
            this.lblMemoryAddress.AutoSize = true;
            this.lblMemoryAddress.Location = new System.Drawing.Point(3, 4);
            this.lblMemoryAddress.Name = "lblMemoryAddress";
            this.lblMemoryAddress.Size = new System.Drawing.Size(45, 13);
            this.lblMemoryAddress.TabIndex = 9;
            this.lblMemoryAddress.Text = "Address";
            // 
            // btnMemoryWrite
            // 
            this.btnMemoryWrite.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnMemoryWrite.Location = new System.Drawing.Point(115, 23);
            this.btnMemoryWrite.Name = "btnMemoryWrite";
            this.btnMemoryWrite.Size = new System.Drawing.Size(72, 23);
            this.btnMemoryWrite.TabIndex = 8;
            this.btnMemoryWrite.Text = "Write";
            this.btnMemoryWrite.UseVisualStyleBackColor = false;
            this.btnMemoryWrite.Click += new System.EventHandler(this.btnMemoryWrite_Click);
            // 
            // numMemoryAddress
            // 
            this.numMemoryAddress.Hexadecimal = true;
            this.numMemoryAddress.Location = new System.Drawing.Point(6, 26);
            this.numMemoryAddress.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numMemoryAddress.Name = "numMemoryAddress";
            this.numMemoryAddress.Size = new System.Drawing.Size(67, 20);
            this.numMemoryAddress.TabIndex = 7;
            this.numMemoryAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbMemoryUpdateByte
            // 
            this.tbMemoryUpdateByte.Location = new System.Drawing.Point(79, 26);
            this.tbMemoryUpdateByte.Name = "tbMemoryUpdateByte";
            this.tbMemoryUpdateByte.Size = new System.Drawing.Size(30, 20);
            this.tbMemoryUpdateByte.TabIndex = 0;
            this.tbMemoryUpdateByte.Text = "00";
            this.tbMemoryUpdateByte.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackColor = System.Drawing.Color.LightGreen;
            this.btnNextPage.Location = new System.Drawing.Point(486, 384);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(49, 23);
            this.btnNextPage.TabIndex = 8;
            this.btnNextPage.Text = "==>";
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.BackColor = System.Drawing.Color.LightGreen;
            this.btnPrevPage.Location = new System.Drawing.Point(436, 384);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(49, 23);
            this.btnPrevPage.TabIndex = 7;
            this.btnPrevPage.Text = "<==";
            this.btnPrevPage.UseVisualStyleBackColor = false;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnMemoryStartAddress
            // 
            this.btnMemoryStartAddress.BackColor = System.Drawing.Color.LightGreen;
            this.btnMemoryStartAddress.Location = new System.Drawing.Point(107, 6);
            this.btnMemoryStartAddress.Name = "btnMemoryStartAddress";
            this.btnMemoryStartAddress.Size = new System.Drawing.Size(83, 23);
            this.btnMemoryStartAddress.TabIndex = 4;
            this.btnMemoryStartAddress.Text = "View Address";
            this.btnMemoryStartAddress.UseVisualStyleBackColor = false;
            this.btnMemoryStartAddress.Click += new System.EventHandler(this.btnMemoryStartAddress_Click);
            // 
            // tbMemoryStartAddress
            // 
            this.tbMemoryStartAddress.Location = new System.Drawing.Point(59, 8);
            this.tbMemoryStartAddress.MaxLength = 4;
            this.tbMemoryStartAddress.Name = "tbMemoryStartAddress";
            this.tbMemoryStartAddress.Size = new System.Drawing.Size(42, 20);
            this.tbMemoryStartAddress.TabIndex = 3;
            this.tbMemoryStartAddress.Text = "0000";
            this.tbMemoryStartAddress.TextChanged += new System.EventHandler(this.tbMemoryStartAddress_TextChanged);
            this.tbMemoryStartAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMemoryStartAddress_KeyPress);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(8, 11);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 2;
            this.lblAddress.Text = "Address";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripSeparator1,
            this.toolStripButtonRestartSimulator,
            this.toolStripSeparator2,
            this.toolStripButtonStartDebug,
            this.toolStripButtonRun,
            this.toolStripButtonFast,
            this.toolStripButtonStep,
            this.toolStripButtonStop});
            this.toolStrip.Location = new System.Drawing.Point(249, 9);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(245, 25);
            this.toolStrip.TabIndex = 18;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNew.Text = "New File";
            this.toolStripButtonNew.Click += new System.EventHandler(this.new_Click);
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = global::JuniorComputer.Properties.Resources.open;
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpen.ToolTipText = "Open File";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.open_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::JuniorComputer.Properties.Resources.save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.save_Click);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveAs.Image = global::JuniorComputer.Properties.Resources.save_as;
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveAs.Text = "Save As";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.saveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRestartSimulator
            // 
            this.toolStripButtonRestartSimulator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRestartSimulator.Image = global::JuniorComputer.Properties.Resources.reset;
            this.toolStripButtonRestartSimulator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRestartSimulator.Name = "toolStripButtonRestartSimulator";
            this.toolStripButtonRestartSimulator.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRestartSimulator.Text = "Reset Simulator";
            this.toolStripButtonRestartSimulator.Click += new System.EventHandler(this.resetSimulator_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonStartDebug
            // 
            this.toolStripButtonStartDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStartDebug.Image = global::JuniorComputer.Properties.Resources.debug;
            this.toolStripButtonStartDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStartDebug.Name = "toolStripButtonStartDebug";
            this.toolStripButtonStartDebug.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStartDebug.Text = "Start Debugging";
            this.toolStripButtonStartDebug.Click += new System.EventHandler(this.startDebug_Click);
            // 
            // toolStripButtonRun
            // 
            this.toolStripButtonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRun.Image = global::JuniorComputer.Properties.Resources.play;
            this.toolStripButtonRun.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripButtonRun.Name = "toolStripButtonRun";
            this.toolStripButtonRun.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRun.Text = "Run";
            this.toolStripButtonRun.Click += new System.EventHandler(this.startRun_Click);
            // 
            // toolStripButtonFast
            // 
            this.toolStripButtonFast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFast.Image = global::JuniorComputer.Properties.Resources.fast;
            this.toolStripButtonFast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFast.Name = "toolStripButtonFast";
            this.toolStripButtonFast.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFast.Text = "Fast";
            this.toolStripButtonFast.Click += new System.EventHandler(this.startFast_Click);
            // 
            // toolStripButtonStep
            // 
            this.toolStripButtonStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStep.Image = global::JuniorComputer.Properties.Resources.step;
            this.toolStripButtonStep.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButtonStep.Name = "toolStripButtonStep";
            this.toolStripButtonStep.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStep.Text = "Step";
            this.toolStripButtonStep.Click += new System.EventHandler(this.startStep_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Image = global::JuniorComputer.Properties.Resources.stop;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.ToolTipText = "Stop";
            this.toolStripButtonStop.Click += new System.EventHandler(this.stop_Click);
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(272, 944);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(13, 13);
            this.lblLine.TabIndex = 20;
            this.lblLine.Text = "0";
            // 
            // lblColumn
            // 
            this.lblColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblColumn.AutoSize = true;
            this.lblColumn.Location = new System.Drawing.Point(314, 944);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(13, 13);
            this.lblColumn.TabIndex = 21;
            this.lblColumn.Text = "0";
            // 
            // btnViewProgram
            // 
            this.btnViewProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewProgram.Location = new System.Drawing.Point(1040, 11);
            this.btnViewProgram.Name = "btnViewProgram";
            this.btnViewProgram.Size = new System.Drawing.Size(82, 23);
            this.btnViewProgram.TabIndex = 22;
            this.btnViewProgram.Text = "View Program";
            this.btnViewProgram.UseVisualStyleBackColor = true;
            this.btnViewProgram.Click += new System.EventHandler(this.btnViewProgram_Click);
            // 
            // btnViewSymbolTable
            // 
            this.btnViewSymbolTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewSymbolTable.Location = new System.Drawing.Point(926, 11);
            this.btnViewSymbolTable.Name = "btnViewSymbolTable";
            this.btnViewSymbolTable.Size = new System.Drawing.Size(108, 23);
            this.btnViewSymbolTable.TabIndex = 23;
            this.btnViewSymbolTable.Text = "View Symbol Table";
            this.btnViewSymbolTable.UseVisualStyleBackColor = true;
            this.btnViewSymbolTable.Click += new System.EventHandler(this.btnViewSymbolTable_Click);
            // 
            // panelMemory
            // 
            this.panelMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMemory.BackColor = System.Drawing.Color.LightGray;
            this.panelMemory.Controls.Add(this.chkLatched);
            this.panelMemory.Controls.Add(this.btnViewSP);
            this.panelMemory.Controls.Add(this.btnViewPC);
            this.panelMemory.Controls.Add(this.chkLock);
            this.panelMemory.Controls.Add(this.panelWriteMemory);
            this.panelMemory.Controls.Add(this.panelMemoryInfo);
            this.panelMemory.Controls.Add(this.lblAddress);
            this.panelMemory.Controls.Add(this.btnPrevPage);
            this.panelMemory.Controls.Add(this.tbMemoryStartAddress);
            this.panelMemory.Controls.Add(this.btnNextPage);
            this.panelMemory.Controls.Add(this.btnMemoryStartAddress);
            this.panelMemory.Location = new System.Drawing.Point(826, 41);
            this.panelMemory.Name = "panelMemory";
            this.panelMemory.Size = new System.Drawing.Size(550, 446);
            this.panelMemory.TabIndex = 24;
            // 
            // chkLatched
            // 
            this.chkLatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLatched.AutoSize = true;
            this.chkLatched.BackColor = System.Drawing.SystemColors.Control;
            this.chkLatched.ForeColor = System.Drawing.Color.Black;
            this.chkLatched.Location = new System.Drawing.Point(436, 418);
            this.chkLatched.Margin = new System.Windows.Forms.Padding(0);
            this.chkLatched.Name = "chkLatched";
            this.chkLatched.Padding = new System.Windows.Forms.Padding(3);
            this.chkLatched.Size = new System.Drawing.Size(96, 23);
            this.chkLatched.TabIndex = 45;
            this.chkLatched.Text = "Latch Display";
            this.chkLatched.UseVisualStyleBackColor = false;
            // 
            // btnViewSP
            // 
            this.btnViewSP.BackColor = System.Drawing.Color.LightGreen;
            this.btnViewSP.Location = new System.Drawing.Point(321, 6);
            this.btnViewSP.Name = "btnViewSP";
            this.btnViewSP.Size = new System.Drawing.Size(119, 23);
            this.btnViewSP.TabIndex = 12;
            this.btnViewSP.Text = "View StackPointer";
            this.btnViewSP.UseVisualStyleBackColor = false;
            this.btnViewSP.Click += new System.EventHandler(this.btnViewSP_Click);
            // 
            // btnViewPC
            // 
            this.btnViewPC.BackColor = System.Drawing.Color.LightGreen;
            this.btnViewPC.Location = new System.Drawing.Point(196, 6);
            this.btnViewPC.Name = "btnViewPC";
            this.btnViewPC.Size = new System.Drawing.Size(119, 23);
            this.btnViewPC.TabIndex = 11;
            this.btnViewPC.Text = "View ProgramCounter";
            this.btnViewPC.UseVisualStyleBackColor = false;
            this.btnViewPC.Click += new System.EventHandler(this.btnViewPC_Click);
            // 
            // chkLock
            // 
            this.chkLock.AutoSize = true;
            this.chkLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLock.Location = new System.Drawing.Point(481, 8);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(59, 20);
            this.chkLock.TabIndex = 10;
            this.chkLock.Text = "Lock";
            this.chkLock.UseVisualStyleBackColor = true;
            // 
            // panelWriteMemory
            // 
            this.panelWriteMemory.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelWriteMemory.Controls.Add(this.lblValueMemory);
            this.panelWriteMemory.Controls.Add(this.btnMemoryWrite);
            this.panelWriteMemory.Controls.Add(this.lblMemoryAddress);
            this.panelWriteMemory.Controls.Add(this.tbMemoryUpdateByte);
            this.panelWriteMemory.Controls.Add(this.numMemoryAddress);
            this.panelWriteMemory.Location = new System.Drawing.Point(6, 386);
            this.panelWriteMemory.Name = "panelWriteMemory";
            this.panelWriteMemory.Size = new System.Drawing.Size(199, 55);
            this.panelWriteMemory.TabIndex = 9;
            // 
            // richTextBoxProgram
            // 
            this.richTextBoxProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxProgram.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxProgram.Location = new System.Drawing.Point(333, 40);
            this.richTextBoxProgram.Name = "richTextBoxProgram";
            this.richTextBoxProgram.Size = new System.Drawing.Size(487, 901);
            this.richTextBoxProgram.TabIndex = 5;
            this.richTextBoxProgram.Text = "";
            this.richTextBoxProgram.WordWrap = false;
            this.richTextBoxProgram.SelectionChanged += new System.EventHandler(this.richTextBoxProgram_SelectionChanged);
            this.richTextBoxProgram.VScroll += new System.EventHandler(this.richTextBoxProgram_VScroll);
            this.richTextBoxProgram.TextChanged += new System.EventHandler(this.richTextBoxProgram_TextChanged);
            this.richTextBoxProgram.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBoxProgram_MouseDown);
            this.richTextBoxProgram.MouseEnter += new System.EventHandler(this.richTextBoxProgram_MouseEnter);
            this.richTextBoxProgram.MouseLeave += new System.EventHandler(this.richTextBoxProgram_MouseLeave);
            this.richTextBoxProgram.MouseMove += new System.Windows.Forms.MouseEventHandler(this.richTextBoxProgram_MouseMove);
            // 
            // btnClearBreakPoint
            // 
            this.btnClearBreakPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearBreakPoint.Location = new System.Drawing.Point(826, 12);
            this.btnClearBreakPoint.Name = "btnClearBreakPoint";
            this.btnClearBreakPoint.Size = new System.Drawing.Size(94, 22);
            this.btnClearBreakPoint.TabIndex = 27;
            this.btnClearBreakPoint.Text = "Clear Breakpoint";
            this.btnClearBreakPoint.UseVisualStyleBackColor = true;
            this.btnClearBreakPoint.Click += new System.EventHandler(this.btnClearBreakPoint_Click);
            // 
            // pbBreakPoint
            // 
            this.pbBreakPoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbBreakPoint.BackColor = System.Drawing.Color.LightGray;
            this.pbBreakPoint.Location = new System.Drawing.Point(308, 40);
            this.pbBreakPoint.Name = "pbBreakPoint";
            this.pbBreakPoint.Size = new System.Drawing.Size(18, 901);
            this.pbBreakPoint.TabIndex = 28;
            this.pbBreakPoint.TabStop = false;
            this.pbBreakPoint.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbBreakPoint_MouseClick);
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDelay.Location = new System.Drawing.Point(1307, 10);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownDelay.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(65, 24);
            this.numericUpDownDelay.TabIndex = 29;
            this.numericUpDownDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownDelay.UseWaitCursor = true;
            this.numericUpDownDelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownDelay.ValueChanged += new System.EventHandler(this.numericUpDownDelay_ValueChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(1230, 18);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(71, 13);
            this.lblDelay.TabIndex = 30;
            this.lblDelay.Text = "Delay (msec):";
            // 
            // lblSetProgramCounter
            // 
            this.lblSetProgramCounter.AutoSize = true;
            this.lblSetProgramCounter.Location = new System.Drawing.Point(667, 16);
            this.lblSetProgramCounter.Name = "lblSetProgramCounter";
            this.lblSetProgramCounter.Size = new System.Drawing.Size(105, 13);
            this.lblSetProgramCounter.TabIndex = 31;
            this.lblSetProgramCounter.Text = "Set Program Counter";
            // 
            // tbSetProgramCounter
            // 
            this.tbSetProgramCounter.Location = new System.Drawing.Point(778, 14);
            this.tbSetProgramCounter.MaxLength = 4;
            this.tbSetProgramCounter.Name = "tbSetProgramCounter";
            this.tbSetProgramCounter.Size = new System.Drawing.Size(42, 20);
            this.tbSetProgramCounter.TabIndex = 32;
            this.tbSetProgramCounter.Text = "0000";
            this.tbSetProgramCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbSetProgramCounter.TextChanged += new System.EventHandler(this.tbSetProgramCounter_TextChanged);
            this.tbSetProgramCounter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSetProgramCounter_KeyPress);
            // 
            // panelInstructions
            // 
            this.panelInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelInstructions.AutoScroll = true;
            this.panelInstructions.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelInstructions.Location = new System.Drawing.Point(12, 186);
            this.panelInstructions.Name = "panelInstructions";
            this.panelInstructions.Size = new System.Drawing.Size(290, 755);
            this.panelInstructions.TabIndex = 44;
            // 
            // ucJunior
            // 
            this.ucJunior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ucJunior.BackColor = System.Drawing.Color.DarkGreen;
            this.ucJunior.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucJunior.BackgroundImage")));
            this.ucJunior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucJunior.Location = new System.Drawing.Point(826, 501);
            this.ucJunior.MaximumSize = new System.Drawing.Size(550, 440);
            this.ucJunior.MinimumSize = new System.Drawing.Size(550, 440);
            this.ucJunior.Name = "ucJunior";
            this.ucJunior.Size = new System.Drawing.Size(550, 440);
            this.ucJunior.TabIndex = 43;
            // 
            // chkInsertMonitor
            // 
            this.chkInsertMonitor.AutoSize = true;
            this.chkInsertMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.chkInsertMonitor.ForeColor = System.Drawing.Color.Black;
            this.chkInsertMonitor.Location = new System.Drawing.Point(506, 15);
            this.chkInsertMonitor.Margin = new System.Windows.Forms.Padding(0);
            this.chkInsertMonitor.Name = "chkInsertMonitor";
            this.chkInsertMonitor.Size = new System.Drawing.Size(140, 17);
            this.chkInsertMonitor.TabIndex = 46;
            this.chkInsertMonitor.Text = "Insert Monitor on Debug";
            this.chkInsertMonitor.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.chkInsertMonitor);
            this.Controls.Add(this.panelInstructions);
            this.Controls.Add(this.ucJunior);
            this.Controls.Add(this.lblSetProgramCounter);
            this.Controls.Add(this.tbSetProgramCounter);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.numericUpDownDelay);
            this.Controls.Add(this.pbBreakPoint);
            this.Controls.Add(this.btnClearBreakPoint);
            this.Controls.Add(this.panelMemory);
            this.Controls.Add(this.btnViewSymbolTable);
            this.Controls.Add(this.btnViewProgram);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.richTextBoxProgram);
            this.Controls.Add(this.groupBoxFlags);
            this.Controls.Add(this.groupBoxRegisters);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1200, 1000);
            this.Name = "MainForm";
            this.Text = "Junior Computer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxFlags.ResumeLayout(false);
            this.groupBoxFlags.PerformLayout();
            this.groupBoxRegisters.ResumeLayout(false);
            this.groupBoxRegisters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMemoryAddress)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelMemory.ResumeLayout(false);
            this.panelMemory.PerformLayout();
            this.panelWriteMemory.ResumeLayout(false);
            this.panelWriteMemory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBreakPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetRAMToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxFlags;
        private System.Windows.Forms.Label lblFlagC;
        private System.Windows.Forms.Label lblFlagB;
        private System.Windows.Forms.Label lblFlagD;
        private System.Windows.Forms.Label lblFlagZ;
        private System.Windows.Forms.Label lblFlagN;
        private System.Windows.Forms.GroupBox groupBoxRegisters;
        private System.Windows.Forms.Label labelSPRegister;
        private System.Windows.Forms.Label labelPCRegister;
        private System.Windows.Forms.Label labelYRegister;
        private System.Windows.Forms.Label labelXRegister;
        private System.Windows.Forms.Label labelARegister;
        private System.Windows.Forms.Label lblSP;
        private System.Windows.Forms.Label lblPC;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Button btnMemoryWrite;
        private System.Windows.Forms.NumericUpDown numMemoryAddress;
        private System.Windows.Forms.TextBox tbMemoryUpdateByte;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Panel panelMemoryInfo;
        private System.Windows.Forms.Button btnMemoryStartAddress;
        private System.Windows.Forms.TextBox tbMemoryStartAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRestartSimulator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonStartDebug;
        private System.Windows.Forms.ToolStripButton toolStripButtonStep;
        private System.Windows.Forms.Label lblValueMemory;
        private System.Windows.Forms.Label lblMemoryAddress;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolTip toolTipRegisterBinary;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.Button btnViewProgram;
        private System.Windows.Forms.Button btnViewSymbolTable;
        private System.Windows.Forms.CheckBox chkFlagN;
        private System.Windows.Forms.CheckBox chkFlagC;
        private System.Windows.Forms.CheckBox chkFlagB;
        private System.Windows.Forms.CheckBox chkFlagD;
        private System.Windows.Forms.CheckBox chkFlagZ;
        private System.Windows.Forms.Panel panelMemory;
        private System.Windows.Forms.Panel panelWriteMemory;
        private System.Windows.Forms.RichTextBox richTextBoxProgram;
        private System.Windows.Forms.ToolStripButton toolStripButtonRun;
        private System.Windows.Forms.Button btnClearBreakPoint;
        private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbBreakPoint;
        private System.Windows.Forms.CheckBox chkFlag1;
        private System.Windows.Forms.Label lblFlag1;
        private System.Windows.Forms.CheckBox chkFlagV;
        private System.Windows.Forms.Label lblFlagV;
        private System.Windows.Forms.ToolStripMenuItem disAssemblerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBinaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveBinaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.CheckBox chkLock;
        private System.Windows.Forms.Label lblSetProgramCounter;
        private System.Windows.Forms.TextBox tbSetProgramCounter;
        private System.Windows.Forms.Button btnViewSP;
        private System.Windows.Forms.Button btnViewPC;
        private UCJunior ucJunior;
        private System.Windows.Forms.CheckBox chkFlagI;
        private System.Windows.Forms.Label lblFlagI;
        private System.Windows.Forms.Panel panelInstructions;
        private System.Windows.Forms.ToolStripButton toolStripButtonFast;
        private System.Windows.Forms.CheckBox chkLatched;
        private System.Windows.Forms.CheckBox chkInsertMonitor;
    }
}

