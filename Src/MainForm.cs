using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JuniorComputer
{
    public partial class MainForm : Form
    {
        #region Members

        // Structure to describe the commands for the assembler
        private struct CommandDescription
        {
            public string Instruction;
            public string Operand;
            public string Description;

            public CommandDescription(string instruction = "", string operand = "", string description = "")
            {
                Instruction = instruction;
                Operand = operand;
                Description = description;
            }
            public override string ToString() => Instruction + " " + Operand + "; " + Description;
        }

        // Assembler object
        private Assembler assembler;
        
        // Rows of memory panel   
        private Label[] memoryAddressLabels = new Label[0x10];

        // Columns of memory panel
        private Label[] memoryAddressIndexLabels = new Label[0x10];

        // Contents of memory panel table
        private Label[,] memoryTableLabels = new Label[0x10, 0x10];

        // File selected for loading/saving 
        private string sourceFile = "";

        // Next instruction address
        private UInt16 nextInstrAddress = 0;

        // Line on which a breakpoint has been set
        private int lineBreakPoint = -1;

        // Tooltip for button/menu items
        private ToolTip toolTip;

        // Delay for running program
        private Timer timer = new Timer();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            toolStripButtonRun.Enabled = false;
            toolStripButtonFast.Enabled = false;
            toolStripButtonStep.Enabled = false;

            pbBreakPoint.Image = new Bitmap(pbBreakPoint.Height, pbBreakPoint.Width);
            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);

            // Scroll memory panel with mousewheel
            this.panelMemory.MouseWheel += PanelMemory_MouseWheel;
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// MainForm loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set location of mainform
            this.Location = new Point(20, 20);

            // Tooltip with line (address) info
            toolTip = new ToolTip();
            toolTip.OwnerDraw = true;
            toolTip.IsBalloon = false;
            toolTip.BackColor = Color.Azure;
            toolTip.Draw += ToolTip_Draw;
            toolTip.Popup += ToolTip_Popup;

            // Create font for header text
            Font font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            // We can view 256 bytes of memory at a time, it will be in form of 16 X 16
            for (int i = 0; i < 0x10; i++)
            {
                Label label = new Label();
                label.Name = "memoryAddressLabel" + i.ToString("X");
                label.Font = font;
                label.Text = (i * 16).ToString("X").PadLeft(4, '0');
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Visible = true;
                label.Size = new System.Drawing.Size(44, 15);
                label.Location = new Point(10, 20 + 20 * i);
                label.BackColor = SystemColors.GradientInactiveCaption;
                panelMemoryInfo.Controls.Add(label);

                memoryAddressLabels[i] = label;
            }

            // MemoryAddressIndexLabels, display the top row required for the memory table
            for (int i = 0; i < 0x10; i++)
            {
                Label label = new Label();
                label.Name = "memoryAddressIndexLabel" + i.ToString("X");
                label.Font = font;
                label.Text = i.ToString("X");
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Visible = true;
                label.Size = new System.Drawing.Size(20, 15);
                label.Location = new Point(60 + 30 * i, 0);
                label.BackColor = SystemColors.GradientInactiveCaption;
                panelMemoryInfo.Controls.Add(label);

                memoryAddressIndexLabels[i] = label;
            }

            // MemoryTableLabels, display the memory contents
            for (int i = 0; i < 0x10; i++)
            {
                for (int j = 0; j < 0x10; j++)
                {
                    Label label = new Label();
                    int address = 16 * i + j;
                    label.Name = "memoryTableLabel" + address.ToString("X").PadLeft(2, '0');
                    label.Text = null;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Visible = true;
                    label.Size = new System.Drawing.Size(24, 15);
                    label.Location = new Point(60 + 30 * j, 20 + 20 * i);
                    panelMemoryInfo.Controls.Add(label);

                    memoryTableLabels[i, j] = label;
                }
            }

            timer.Interval = Convert.ToInt32(numericUpDownDelay.Value);
            timer.Tick += new EventHandler(TimerEventProcessor);

            // Initialize the buttons (add a tag with info)
            InitButtons();
        }

        /// <summary>
        /// Timer event handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimerEventProcessor(Object obj, EventArgs myEventArgs)
        {
            Timer timer = (Timer)obj;

            UInt16 currentInstrAddress = nextInstrAddress;

            string error = assembler.RunInstruction(currentInstrAddress, ref nextInstrAddress);

            // Check step switch
            if (ucJunior.step && (currentInstrAddress < 0x1C00) && (currentInstrAddress >= 0x2000)) NMI();

            UInt16 startViewAddress = Convert.ToUInt16(memoryAddressLabels[0].Text, 16);

            if (!chkLock.Checked)
            {
                if (nextInstrAddress > startViewAddress + 0x100) startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
                if (nextInstrAddress < startViewAddress)         startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
            }

            UpdateMemoryPanel(startViewAddress, nextInstrAddress);
            UpdateRegisters();
            UpdateFlags();
            UpdateDisplay();
            UpdateKeyboard();

            if (error == "")
            {
                ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], false);

                if (assembler.RAMprogramLine[nextInstrAddress] == lineBreakPoint)
                {
                    timer.Enabled = false;

                    // Enable event handler for updating row/column 
                    richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);

                    ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);
                    if (chkLock.Checked)
                    {
                        UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);
                    } else
                    {
                        UpdateMemoryPanel(currentInstrAddress, nextInstrAddress);
                    }
                    UpdateRegisters();
                    UpdateFlags();

                    toolStripButtonRun.Enabled = true;
                    toolStripButtonFast.Enabled = true;
                    toolStripButtonStep.Enabled = true;
                }
            } else
            {
                timer.Enabled = false;

                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;

                // Enable event handler for updating row/column 
                richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);

                if (error == "System Halted")
                {
                    ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], false);
                    MessageBox.Show(error, "SYSTEM HALTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], true);
                    MessageBox.Show(error, "RUNTIME ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Memory startaddress changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMemoryStartAddress_TextChanged(object sender, EventArgs e)
        {
            string hexdigits = "1234567890ABCDEFabcdef";
            bool noHex = false;
            foreach (char c in tbMemoryStartAddress.Text)
            {
                if (hexdigits.IndexOf(c) < 0)
                {
                    MessageBox.Show("Only hexadecimal values", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    noHex = true;
                }
            }

            if (noHex) tbMemoryStartAddress.Text = "0000";
        }

        /// <summary>
        /// View memory from this address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMemoryStartAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);
            }
        }

        /// <summary>
        /// Startaddress changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSetProgramCounter_TextChanged(object sender, EventArgs e)
        {
            string hexdigits = "1234567890ABCDEFabcdef";
            bool noHex = false;
            foreach (char c in tbSetProgramCounter.Text)
            {
                if (hexdigits.IndexOf(c) < 0)
                {
                    MessageBox.Show("Only hexadecimal values", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    noHex = true;
                }
            }

            if (noHex) tbSetProgramCounter.Text = "0000";
        }

        /// <summary>
        /// Startaddress changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSetProgramCounter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && (assembler != null))
            {
                nextInstrAddress = Convert.ToUInt16(tbSetProgramCounter.Text, 16);
                labelPCRegister.Text = tbSetProgramCounter.Text;

                ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);

                if (!chkLock.Checked) UpdateMemoryPanel(nextInstrAddress, nextInstrAddress);
            }
        }

        /// <summary>
        /// Timer delay while running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownDelay_ValueChanged(object sender, EventArgs e)
        {
            timer.Interval = Convert.ToInt32(numericUpDownDelay.Value);
        }

        /// <summary>
        /// Add/Change breakpoint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void pbBreakPoint_MouseClick(object sender, MouseEventArgs e)
        {
            // Get character index of mouse Y position in current program
            int index = richTextBoxProgram.GetCharIndexFromPosition(new Point(0, e.Y));

            // Get line number
            lineBreakPoint = richTextBoxProgram.GetLineFromCharIndex(index);

            // Set (update) breakpoint on screen
            UpdateBreakPoint(lineBreakPoint);
        }

        /// <summary>
        /// Main form resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // Set (update) breakpoint on screen
            UpdateBreakPoint(lineBreakPoint);
        }

        /// <summary>
        /// Draw tooltip with specific font
        /// </summary>
        private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Font font = new Font(FontFamily.GenericMonospace, 12.0f);
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, font, Brushes.Black, new Point(2, 2));
        }

        /// <summary>
        /// Set font for the tooltip popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            Font font = new Font(FontFamily.GenericMonospace, 12.0f);
            Size size = TextRenderer.MeasureText(toolTip.GetToolTip(e.AssociatedControl), font);
            e.ToolTipSize = new Size(size.Width + 3, size.Height + 3);
        }

        /// <summary>
        /// Change memory view range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMemory_MouseWheel(object sender, MouseEventArgs e)
        {
            if (assembler != null)
            {
                if (e.Delta < 0)
                {
                    if (Convert.ToUInt16(memoryAddressLabels[0].Text, 16) < 0xFFF0)
                    {
                        UInt16 n = (UInt16)(Convert.ToUInt16(memoryAddressLabels[0].Text, 16) + 0x0010);

                        tbMemoryStartAddress.Text = n.ToString("X4");
                        UpdateMemoryPanel(n, nextInstrAddress);
                    }
                }

                if (e.Delta > 0)
                {
                    if (Convert.ToUInt16(memoryAddressLabels[0].Text, 16) >= 0x0010)
                    {
                        UInt16 n = (UInt16)(Convert.ToUInt16(memoryAddressLabels[0].Text, 16) - 0x0010);

                        tbMemoryStartAddress.Text = n.ToString("X4");
                        UpdateMemoryPanel(n, nextInstrAddress);
                    }
                }
            }
        }

        /// <summary>
        /// Change negative flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagN_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagN = chkFlagN.Checked;
        }

        /// <summary>
        /// Change overflow flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagV_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagV = chkFlagV.Checked;
        }

        /// <summary>
        /// Change zero flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagZ_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagZ = chkFlagZ.Checked;
        }

        /// <summary>
        /// Change interrupt flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagI_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagI = chkFlagI.Checked;
        }

        /// <summary>
        /// Change decimal flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagD_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagD = chkFlagD.Checked;
        }

        /// <summary>
        /// Change break flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagB_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagB = chkFlagB.Checked;
        }

        /// <summary>
        /// Change carry flag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFlagC_CheckedChanged(object sender, EventArgs e)
        {
            if (assembler != null) assembler.flagC = chkFlagC.Checked;
        }

        /// <summary>
        /// Show tooltiptext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseHover(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Instruction instruction = (Instruction)control.Tag;

            string addressing = "";
            switch (instruction.Addressing)
            {
                case ADDRESSING.ABSOLUTE:
                    addressing = "Absolute: operand is address nn";
                    break;
                case ADDRESSING.ABSOLUTEX:
                    addressing = "Absolute, X-indexed: effective address is address nn incremented by X";
                    break;
                case ADDRESSING.ABSOLUTEY:
                    addressing = "Absolute, Y-indexed: effective address is address nn incremented by Y";
                    break;
                case ADDRESSING.IMMEDIATE:
                    addressing = "Immediate: operand is byte n";
                    break;
                case ADDRESSING.IMPLIED:
                    addressing = "Implied: operand is implied";
                    break;
                case ADDRESSING.INDIRECT:
                    addressing = "Indirect: effective address is contents of word at address";
                    break;
                case ADDRESSING.XINDIRECT:
                    addressing = "X-indexed, Indirect: effective address is contents of word at address + X";
                    break;
                case ADDRESSING.INDIRECTY:
                    addressing = "Indirect, Y-indexed: effective address is contents of word + Y at address";
                    break;
                case ADDRESSING.RELATIVE:
                    addressing = "Relative: branch target is PC + signed offset n";
                    break;
                case ADDRESSING.ZEROPAGE:
                    addressing = "Zeropage: zeropage address n";
                    break;
                case ADDRESSING.ZEROPAGEX:
                    addressing = "Zeropage: zeropage address n + X";
                    break;
                default:
                    addressing = "Unknown";
                    break;
            }

            toolTip.SetToolTip(control, instruction.Description + "\r\nAdressing Mode = " + addressing);
            toolTip.Active = true;
        }

        #endregion

        #region EventHandlers (Menu)

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select Assembly File";
            fileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            fileDialog.FileName = "";
            fileDialog.Filter = "6502 assembly|*.asm;|All Files|*.*";

            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                sourceFile = fileDialog.FileName;
                System.IO.StreamReader asmProgramReader;
                asmProgramReader = new System.IO.StreamReader(sourceFile);
                richTextBoxProgram.Text = asmProgramReader.ReadToEnd();
                asmProgramReader.Close();
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (sourceFile == "")
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Save File As";
                fileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                fileDialog.FileName = "";
                fileDialog.Filter = "6502 assembly|*.asm|All Files|*.*";

                if (fileDialog.ShowDialog() != DialogResult.Cancel)
                {
                    sourceFile = fileDialog.FileName;
                    System.IO.StreamWriter asmProgramWriter;
                    asmProgramWriter = new System.IO.StreamWriter(sourceFile);
                    asmProgramWriter.Write(richTextBoxProgram.Text);
                    asmProgramWriter.Close();
                }
            } else
            {
                System.IO.StreamWriter asmProgramWriter;
                asmProgramWriter = new System.IO.StreamWriter(sourceFile);
                asmProgramWriter.Write(richTextBoxProgram.Text);
                asmProgramWriter.Close();
            }
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Save File As";
            fileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            fileDialog.FileName = "";
            fileDialog.Filter = "6502 assembly|*.asm|All Files|*.*";

            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                sourceFile = fileDialog.FileName;
                System.IO.StreamWriter asmProgramWriter;
                asmProgramWriter = new System.IO.StreamWriter(sourceFile);
                asmProgramWriter.Write(richTextBoxProgram.Text);
                asmProgramWriter.Close();
            }
        }

        private void saveBinary_Click(object sender, EventArgs e)
        {
            if ((assembler == null) || (assembler.programRun == null))
            {
                MessageBox.Show("Nothing yet to save", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int start = -1;
            int end = -1;

            // Create form for display of results                
            Form addressForm = new Form();
            addressForm.Name = "FormSaveBinary";
            addressForm.Text = "Save Binary";
            addressForm.Icon = Properties.Resources.JuniorComputer;
            addressForm.Size = new Size(200, 160);
            addressForm.MinimumSize = new Size(200, 160);
            addressForm.MaximumSize = new Size(200, 160);
            addressForm.MaximizeBox = false;
            addressForm.MinimizeBox = false;
            addressForm.StartPosition = FormStartPosition.CenterScreen;

            // Create buttons for closing (dialog)form
            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new Point(100, 90);
            btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnOk.Visible = true;
            btnOk.DialogResult = DialogResult.OK;

            // Create buttons for closing (dialog)form
            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(10, 90);
            btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnCancel.Visible = true;
            btnCancel.DialogResult = DialogResult.Cancel;

            // Add controls to form
            Label labelFrom = new Label();
            labelFrom.Text = "From:";
            labelFrom.Location = new Point(10, 10);

            TextBox textBoxFrom = new TextBox();
            textBoxFrom.BackColor = Color.LightYellow;
            textBoxFrom.Size = new Size(50, 24);
            textBoxFrom.Text = "0000";
            textBoxFrom.TextAlign = HorizontalAlignment.Center;
            textBoxFrom.Location = new Point(labelFrom.Width + 10, 10);
            textBoxFrom.TextChanged += new EventHandler(tbAddress_TextChanged);

            // Add controls to form
            Label labelTo = new Label();
            labelTo.Text = "To:";
            labelTo.Location = new Point(10, 40);

            TextBox textBoxTo = new TextBox();
            textBoxTo.BackColor = Color.LightYellow;
            textBoxTo.Size = new Size(50, 24);
            textBoxTo.Text = "0000";
            textBoxTo.TextAlign = HorizontalAlignment.Center;
            textBoxTo.Location = new Point(labelTo.Width + 10, 40);
            textBoxTo.TextChanged += new EventHandler(tbAddress_TextChanged);

            addressForm.Controls.Add(labelFrom);
            addressForm.Controls.Add(labelTo);
            addressForm.Controls.Add(textBoxFrom);
            addressForm.Controls.Add(textBoxTo);
            addressForm.Controls.Add(btnOk);
            addressForm.Controls.Add(btnCancel);

            // Show form
            DialogResult dialogResult = addressForm.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            if ((Convert.ToUInt64(textBoxFrom.Text, 16) > 0xFFFF) || (Convert.ToUInt64(textBoxTo.Text, 16) > 0xFFFF))
            {
                MessageBox.Show("Addresses must be between 0000 and FFFF", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            start = Convert.ToUInt16(textBoxFrom.Text, 16);
            end = Convert.ToUInt16(textBoxTo.Text, 16);

            if (start > end)
            {
                MessageBox.Show("Start must be smaller then end address", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // New byte array with only used code 
            byte[] bytes = new byte[end - start + 1];
            for (int i = 0; i <= end - start; i++)
            {
                bytes[i] = assembler.RAM[start + i];
            }

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "Save Binary File As";
            fileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            fileDialog.FileName = "";
            fileDialog.Filter = "Binary|*.bin|All Files|*.*";

            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                // Save binary file
                File.WriteAllBytes(fileDialog.FileName, bytes);

                MessageBox.Show("Binary file saved as\r\n" + fileDialog.FileName, "SAVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            string hexdigits = "1234567890ABCDEFabcdef";
            bool noHex = false;
            foreach (char c in tbSetProgramCounter.Text)
            {
                if (hexdigits.IndexOf(c) < 0)
                {
                    MessageBox.Show("Only hexadecimal values", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    noHex = true;
                }
            }

            if (noHex) textBox.Text = "0000";
        }

        private void openBinary_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select Binary File";
            fileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            fileDialog.FileName = "";
            fileDialog.Filter = "6502 binary|*.bin|All Files|*.*";

            if (fileDialog.ShowDialog() != DialogResult.Cancel)
            {
                sourceFile = fileDialog.FileName;
                byte[] bytes = File.ReadAllBytes(sourceFile);

                FormAddresses formAddresses = new FormAddresses();
                formAddresses.ShowDialog();

                FormDisAssembler disAssemblerForm = new FormDisAssembler(bytes, formAddresses.loadAddress, formAddresses.startAddress, formAddresses.useLabels);
                DialogResult dialogResult = disAssemblerForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    richTextBoxProgram.Text = disAssemblerForm.program;
                }
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resetSimulator_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;

                // Enable event handler for updating row/column 
                richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);
            }

            assembler = null;
            UpdateMemoryPanel(0x0000, 0x0000);
            UpdateRegisters();
            UpdateFlags();
            ClearDisplay();

            // Reset color
            richTextBoxProgram.SelectionStart = 0;
            richTextBoxProgram.SelectionLength = richTextBoxProgram.Text.Length;
            richTextBoxProgram.SelectionBackColor = System.Drawing.Color.White;

            tbSetProgramCounter.Text = "0000";
            tbMemoryStartAddress.Text = "0000";
            tbMemoryUpdateByte.Text = "00";
            numMemoryAddress.Value = 0000;

            toolStripButtonRun.Enabled = false;
            toolStripButtonFast.Enabled = false;
            toolStripButtonStep.Enabled = false;

            lineBreakPoint = -1;

            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);
        }

        private void resetRAM_Click(object sender, EventArgs e)
        {
            assembler.ClearRam();
            nextInstrAddress = Convert.ToUInt16(tbMemoryStartAddress.Text, 16);
            UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);
        }

        private void new_Click(object sender, EventArgs e)
        {
            assembler = null;
            UpdateMemoryPanel(0x0000, 0x0000);
            UpdateRegisters();
            UpdateFlags();
            ClearDisplay();

            richTextBoxProgram.Clear();
            sourceFile = "";

            tbSetProgramCounter.Text = "0000";
            tbMemoryStartAddress.Text = "0000";
            tbMemoryUpdateByte.Text = "00";
            numMemoryAddress.Value = 0000;

            toolStripButtonRun.Enabled = false;
            toolStripButtonFast.Enabled = false;
            toolStripButtonStep.Enabled = false;

            lineBreakPoint = -1;

            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);
        }

        private void startDebug_Click(object sender, EventArgs e)
        {
            assembler = new Assembler(richTextBoxProgram.Lines);
            nextInstrAddress = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Run the first Pass of assembler
                string message = assembler.FirstPass();
                if (message != "OK")
                {
                    MessageBox.Show(this, message, "FIRSTPASS", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Check if a linenumber has been given
                    string[] fields = message.Split(' ');
                    bool result = Int32.TryParse(fields[fields.Length - 1], out int line);
                    if (result)
                    {
                        // Show where the error is (remember the linenumber returned starts with 1 in stead of 0)
                        ChangeColorRTBLine(line - 1, true);
                    }

                    Cursor.Current = Cursors.Arrow;
                    return;
                }

                // Run second pass
                message = assembler.SecondPass();
                if (message != "OK")
                {
                    MessageBox.Show(this, message, "SECONDPASS", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Show Updated memory
                    UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);

                    // Check if a linenumber has been given
                    string[] fields = message.Split(' ');
                    bool result = Int32.TryParse(fields[fields.Length - 1], out int line);
                    if (result)
                    {
                        // Show where the error is (remember the linenumber returned starts with 1 in stead of 0)
                        ChangeColorRTBLine(line - 1, true);
                    }

                    Cursor.Current = Cursors.Arrow;
                    return;
                }

                // Get startadres of program execution (actually at 0xFFFC and 0xFFFD)
                int address = assembler.RAM[0x1FFC] + 0x100 * assembler.RAM[0x1FFD];
                tbMemoryStartAddress.Text = address.ToString("X4");
                tbSetProgramCounter.Text = address.ToString("X4");

                // Show Updated memory
                UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);

            } catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "startDebug_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Arrow;
                return;
            }

            Cursor.Current = Cursors.Arrow;

            // If monitor should be inserted do it and check for overlap
            if (chkInsertMonitor.Checked)
            {
                // Check if current program overlaps
                bool overlap = false;
                for (int i = 0x1C00; i < 0x2000; i++)
                {
                    if (assembler.RAMprogramLine[i] >= 0)
                    {
                        overlap = true;
                    }
                }

                if (overlap) MessageBox.Show("The monitor program (0x1C00 to 0x2000) will overwrite (some of) the user program", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                byte[] bytes = Properties.Resources.monitor;
                int index = 0x1C00;
                foreach (byte bt in bytes)
                {
                    assembler.RAM[index] = bt;

                    // Indicate this is not a regular program line but also not an invalid one (-1)
                    assembler.RAMprogramLine[index] = -2;

                    index++;
                }
            }

            // Update start address
            if (tbSetProgramCounter.Text == "0000")
            {
                int startline = -1;
                for (int index = 0; (index < assembler.RAMprogramLine.Length) && (startline == -1); index++)
                {
                    if (assembler.RAMprogramLine[index] != -1) startline = index;
                }

                if (startline != -1)
                {
                    tbMemoryStartAddress.Text = startline.ToString("X4");
                    tbSetProgramCounter.Text = startline.ToString("X4");
                }

                if (startline == 0x1C00)
                {
                    tbMemoryStartAddress.Text = "1C1D";
                    tbSetProgramCounter.Text = "1C1D";
                }
            }

            nextInstrAddress = Convert.ToUInt16(tbSetProgramCounter.Text, 16);
            ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);

            UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);
            UpdateRegisters();
            UpdateFlags();
            ClearDisplay();

            toolStripButtonRun.Enabled = true;
            toolStripButtonFast.Enabled = true;
            toolStripButtonStep.Enabled = true;
        }

        private void startRun_Click(object sender, EventArgs e)
        {
            toolStripButtonRun.Enabled = false;
            toolStripButtonFast.Enabled = false;
            toolStripButtonStep.Enabled = false;
            toolStripButtonStop.Enabled = true;

            // Disable event handler for updating row/column 
            richTextBoxProgram.SelectionChanged -= richTextBoxProgram_SelectionChanged;

            timer.Interval = Convert.ToInt32(numericUpDownDelay.Value);
            timer.Enabled = true;
        }

        private void startStep_Click(object sender, EventArgs e)
        {
            UInt16 currentInstrAddress = nextInstrAddress;
            string error = assembler.RunInstruction(currentInstrAddress, ref nextInstrAddress);

            UInt16 startViewAddress = Convert.ToUInt16(memoryAddressLabels[0].Text, 16);

            if (!chkLock.Checked)
            {
                if (nextInstrAddress > startViewAddress + 0x100) startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
                if (nextInstrAddress < startViewAddress)         startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
            }

            UpdateMemoryPanel(startViewAddress, nextInstrAddress);
            UpdateRegisters();
            UpdateFlags();
            UpdateDisplay();
            UpdateKeyboard();

            if (error == "")
            {
                ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);
            } else if (error == "System Halted")
            {
                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;

                ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], false);
                MessageBox.Show(error, "SYSTEM HALTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else 
            {
                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;

                ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], true);
                MessageBox.Show(error, "RUNTIME ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Get index of cursor in current program
            int index = richTextBoxProgram.SelectionStart;

            // Get line number
            int line = richTextBoxProgram.GetLineFromCharIndex(index);
            lblLine.Text = (line + 1).ToString();

            int column = richTextBoxProgram.SelectionStart - richTextBoxProgram.GetFirstCharIndexFromLine(line);
            lblColumn.Text = (column + 1).ToString();
        }

        /// <summary>
        /// Fast run, no updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startFast_Click(object sender, EventArgs e)
        {
            toolStripButtonRun.Enabled = false;
            toolStripButtonFast.Enabled = false;
            toolStripButtonStep.Enabled = false;
            toolStripButtonStop.Enabled = true;

            ClearColorRTBLine();

            string error = "";
            UInt16 currentInstrAddress = nextInstrAddress;

            while (!toolStripButtonFast.Enabled  && (error == ""))
            {
                currentInstrAddress = nextInstrAddress;
                error = assembler.RunInstruction(currentInstrAddress, ref nextInstrAddress);
                if (error == "")
                {
                    // Check step switch
                    if (ucJunior.step && (currentInstrAddress < 0x1C00) && (currentInstrAddress >= 0x2000)) NMI();

                    UpdateDisplay();
                    UpdateKeyboard();
                    if ((assembler.RAMprogramLine[nextInstrAddress] == lineBreakPoint) && (lineBreakPoint != -1))
                    {
                        toolStripButtonRun.Enabled = true;
                        toolStripButtonFast.Enabled = true;
                        toolStripButtonStep.Enabled = true;
                        toolStripButtonStop.Enabled = false;
                    }

                    toolStripButtonStop.Enabled = true;
                    Application.DoEvents();
                }
            }
            UInt16 startViewAddress = Convert.ToUInt16(memoryAddressLabels[0].Text, 16);

            if (!chkLock.Checked)
            {
                if (nextInstrAddress > startViewAddress + 0x100) startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
                if (nextInstrAddress < startViewAddress)         startViewAddress = (UInt16)(nextInstrAddress & 0xFFF0);
            }

            UpdateMemoryPanel(startViewAddress, nextInstrAddress);
            UpdateDisplay();
            UpdateKeyboard();
            UpdateRegisters();
            UpdateFlags();

            if (error == "")
            {
                ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);
                toolStripButtonRun.Enabled = true;
                toolStripButtonFast.Enabled = true;
                toolStripButtonStep.Enabled = true;
                toolStripButtonStop.Enabled = false;
            } else if (error == "System Halted")
            {
                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;
                toolStripButtonStop.Enabled = false;

                ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], false);
                MessageBox.Show(error, "SYSTEM HALTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;
                toolStripButtonStop.Enabled = false;

                ChangeColorRTBLine(assembler.RAMprogramLine[currentInstrAddress], true);
                MessageBox.Show(error, "RUNTIME ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Get index of cursor in current program
            int index = richTextBoxProgram.SelectionStart;

            // Get line/column number
            int line = richTextBoxProgram.GetLineFromCharIndex(index);
            lblLine.Text = (line + 1).ToString();

            int column = richTextBoxProgram.SelectionStart - richTextBoxProgram.GetFirstCharIndexFromLine(line);
            lblColumn.Text = (column + 1).ToString();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                timer.Enabled = false;

                // Enable event handler for updating row/column 
                richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);

                ChangeColorRTBLine(assembler.RAMprogramLine[nextInstrAddress], false);

                toolStripButtonRun.Enabled = true;
                toolStripButtonFast.Enabled = true;
                toolStripButtonStep.Enabled = true;
            }
        }

        private void viewHelp_Click(object sender, EventArgs e)
        {
            FormHelp formHelp = new FormHelp();
            formHelp.ShowDialog();
        }

        private void about_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        #endregion

        #region EventHandlers (Labels)

        private void labelARegister_MouseHover(object sender, EventArgs e)
        {
            RegisterHoverBinary(labelARegister);
        }

        private void labelXRegister_MouseHover(object sender, EventArgs e)
        {
            RegisterHoverBinary(labelXRegister);
        }

        private void labelYRegister_MouseHover(object sender, EventArgs e)
        {
            RegisterHoverBinary(labelYRegister);
        }

        private void labelPCRegister_MouseHover(object sender, EventArgs e)
        {
            RegisterHoverBinary(labelPCRegister);
        }

        private void labelSPRegister_MouseHover(object sender, EventArgs e)
        {
            RegisterHoverBinary(labelSPRegister);
        }

        #endregion

        #region EventHandlers (Buttons)

        /// <summary>
        /// Command buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommand_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Tag != null)
            {
                Instruction instruction = (Instruction)button.Tag;

                if (richTextBoxProgram.SelectionStart == 0)
                {
                    richTextBoxProgram.AppendText(instruction.Mnemonic);
                } else
                {
                    richTextBoxProgram.AppendText(Environment.NewLine + instruction.Mnemonic);
                }
            }
        }

        /// <summary>
        /// View symbol table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewSymbolTable_Click(object sender, EventArgs e)
        {
            if ((assembler != null) && (assembler.programRun != null))
            {
                string addressSymbolTable = "";

                // Check max length of labels
                int maxLabelSize = 0;
                foreach (KeyValuePair<string, int> keyValuePair in assembler.addressSymbolTable)
                {
                    if (keyValuePair.Key.Length > maxLabelSize) maxLabelSize = keyValuePair.Key.Length;
                }

                // Add to table
                foreach (KeyValuePair<string, int> keyValuePair in assembler.addressSymbolTable)
                {
                    addressSymbolTable += keyValuePair.Key;
                    for (int i=keyValuePair.Key.Length; i< maxLabelSize + 1; i++)
                    {
                        addressSymbolTable += " ";
                    }

                    addressSymbolTable += ": " + keyValuePair.Value.ToString("X4") + "\r\n";
                }

                // Create form for display of results                
                Form addressForm = new Form();
                addressForm.Name = "FormSymbolTable";
                addressForm.Text = "SymbolTable";
                addressForm.ShowIcon = false;
                addressForm.Size = new Size(300, 600);
                addressForm.MinimumSize = new Size(300, 600);
                addressForm.MaximumSize = new Size(300, 600);
                addressForm.MaximizeBox = false;
                addressForm.MinimizeBox = false;
                addressForm.StartPosition = FormStartPosition.CenterScreen;

                // Create button for closing (dialog)form
                Button btnOk = new Button();
                btnOk.Text = "OK";
                btnOk.Location = new Point(204, 530);
                btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                btnOk.Visible = true;
                btnOk.Click += new EventHandler((object o, EventArgs a) =>
                {
                    addressForm.Close();
                });

                Font font = new Font(FontFamily.GenericMonospace, 10.25F);

                // Sort alphabetically
                string[] tempArray = addressSymbolTable.Split('\n');
                Array.Sort(tempArray, StringComparer.InvariantCulture);
                addressSymbolTable = "";
                foreach (string line in tempArray)
                {
                    addressSymbolTable += line + '\n';
                }

                // Add controls to form
                TextBox textBox = new TextBox();
                textBox.Multiline = true;
                textBox.WordWrap = false;
                textBox.ScrollBars = ScrollBars.Vertical;
                textBox.ReadOnly = true;
                textBox.BackColor = Color.LightYellow;
                textBox.Size = new Size(268, 510);
                textBox.Text = addressSymbolTable;
                textBox.Font = font;
                textBox.BorderStyle = BorderStyle.None;
                textBox.Location = new Point(10, 10);
                textBox.Select(0, 0);

                addressForm.Controls.Add(textBox);
                addressForm.Controls.Add(btnOk);

                // Show form
                addressForm.Show();
            }
        }

        /// <summary>
        /// View program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewProgram_Click(object sender, EventArgs e)
        {
            if ((assembler != null) && (assembler.programRun != null))
            {
                // Create form for display of results                
                Form formProgram = new Form();
                formProgram.Name = "FormProgram";
                formProgram.Text = "Program";
                formProgram.ShowIcon = false;
                formProgram.Size = new Size(500, 600);
                formProgram.MinimumSize = new Size(500, 600);
                formProgram.MaximizeBox = false;
                formProgram.MinimizeBox = false;
                formProgram.StartPosition = FormStartPosition.CenterScreen;

                // Create button for closing (dialog)form
                Button btnOk = new Button();
                btnOk.Text = "OK";
                btnOk.Location = new Point(400, 530);
                btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                btnOk.Visible = true;
                btnOk.Click += new EventHandler((object o, EventArgs a) =>
                {
                    formProgram.Close();
                });

                string program = "";
                foreach (string line in assembler.programView)
                {
                    if ((line != null) && (line != "") && (line != "\r") && (line != "\n") && (line != "\r\n")) program += line + "\r\n";
                }

                Font font = new Font(FontFamily.GenericMonospace, 10.25F);

                // Add controls to form
                TextBox textBox = new TextBox();
                textBox.Multiline = true;
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                textBox.WordWrap = false;
                textBox.ScrollBars = ScrollBars.Vertical;
                textBox.ReadOnly = true;
                textBox.BackColor = Color.LightYellow;
                textBox.Size = new Size(464, 510);
                textBox.Text = program;
                textBox.Font = font;
                textBox.BorderStyle = BorderStyle.None;
                textBox.Location = new Point(10, 10);
                textBox.Select(0, 0);

                formProgram.Controls.Add(textBox);
                formProgram.Controls.Add(btnOk);

                // Show form
                formProgram.Show();
            }
        }

        /// <summary>
        /// Set memory start address to view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryStartAddress_Click(object sender, EventArgs e)
        {
            UpdateMemoryPanel(GetTextBoxMemoryStartAddress(), nextInstrAddress);
        }

        /// <summary>
        /// Set memory start address to view to Program Counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewPC_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                UpdateMemoryPanel(assembler.registerPC, nextInstrAddress);
                tbMemoryStartAddress.Text = assembler.registerPC.ToString("X4");
            }
        }

        /// <summary>
        /// Set memory start address to view to Stack Pointer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewSP_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                UpdateMemoryPanel((UInt16)(0x0100 + assembler.registerSP), nextInstrAddress);
                tbMemoryStartAddress.Text = (0x0100 + assembler.registerSP).ToString("X4");
            }
        }

        /// <summary>
        /// Previous memory to view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                if (Convert.ToUInt16(memoryAddressLabels[0].Text, 16) >= 0x0100)
                {
                    UInt16 n = (UInt16)(Convert.ToUInt16(memoryAddressLabels[0].Text, 16) - 0x0100);

                    tbMemoryStartAddress.Text = n.ToString("X4");
                    UpdateMemoryPanel(n, nextInstrAddress);
                }
            }
        }

        /// <summary>
        /// Next memory to view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                if (Convert.ToUInt16(memoryAddressLabels[0].Text, 16) < 0xFF00)
                {
                    UInt16 n = (UInt16)(Convert.ToUInt16(memoryAddressLabels[0].Text, 16) + 0x0100);

                    tbMemoryStartAddress.Text = n.ToString("X4");
                    UpdateMemoryPanel(n, nextInstrAddress);
                }
            }
        }

        /// <summary>
        /// Write value to memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryWrite_Click(object sender, EventArgs e)
        {
            if (assembler != null)
            {
                assembler.RAM[(int)numMemoryAddress.Value] = Convert.ToByte(tbMemoryUpdateByte.Text, 16);

                UInt16 n = (UInt16)(Convert.ToUInt16(memoryAddressLabels[0].Text, 16));
                if (
                    (((UInt16)numMemoryAddress.Value) >= n) &&
                    (((UInt16)numMemoryAddress.Value) < n + 0x100)
                   )
                {
                    UpdateMemoryPanel(n, nextInstrAddress);
                }
            }
        }

        /// <summary>
        /// Clear breakpoint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearBreakPoint_Click(object sender, EventArgs e)
        {
            lineBreakPoint = -1;

            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);
        }

        #endregion

        #region EventHandlers (RichTextBox)

        private void richTextBoxProgram_SelectionChanged(object sender, EventArgs e)
        {
            // Get index of cursor in current program
            int index = richTextBoxProgram.SelectionStart;

            // Get line number
            int line = richTextBoxProgram.GetLineFromCharIndex(index);
            lblLine.Text = (line + 1).ToString();

            int column = richTextBoxProgram.SelectionStart - richTextBoxProgram.GetFirstCharIndexFromLine(line);
            lblColumn.Text = (column + 1).ToString();
        }

        // Program adjusted, remove highlight
        private void richTextBoxProgram_TextChanged(object sender, EventArgs e)
        {
            if (toolStripButtonRun.Enabled)
            {
                int pos = richTextBoxProgram.SelectionStart;

                // Reset color
                richTextBoxProgram.SelectionStart = 0;
                richTextBoxProgram.SelectionLength = richTextBoxProgram.Text.Length;
                richTextBoxProgram.SelectionBackColor = System.Drawing.Color.White;

                richTextBoxProgram.SelectionLength = 0;

                richTextBoxProgram.SelectionStart = pos;

                toolStripButtonRun.Enabled = false;
                toolStripButtonFast.Enabled = false;
                toolStripButtonStep.Enabled = false;
            }

            lineBreakPoint = -1;

            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);
        }

        /// <summary>
        /// Mouse button clicked in control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBoxProgram_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;

            int charIndex = richTextBoxProgram.GetCharIndexFromPosition(new Point(x, y));
            int lineIndex = richTextBoxProgram.GetLineFromCharIndex(charIndex);

            if (assembler != null)
            {
                bool found = false;
                for (int address = 0; (address < assembler.RAMprogramLine.Length) && !found; address++)
                {
                    if (assembler.RAMprogramLine[address] == lineIndex)
                    {
                        found = true;
                        int startAddress = Convert.ToInt32(memoryAddressLabels[0].Text, 16);

                        int row = (address - startAddress) / 16;
                        int col = (address - startAddress) % 16;

                        foreach (Label lbl in memoryTableLabels)
                        {
                            if (lbl.BackColor != Color.LightGreen) lbl.BackColor = SystemColors.Info;
                        }

                        if ((row >= 0) && (col >= 0) && (row < 16) && (col < 16))
                        {
                            if (memoryTableLabels[row, col].BackColor != Color.LightGreen) memoryTableLabels[row, col].BackColor = SystemColors.GradientInactiveCaption;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Mouse enters control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBoxProgram_MouseEnter(object sender, EventArgs e)
        {
            toolTip.Active = true;
        }

        /// <summary>
        /// Mouse leaves control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBoxProgram_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(richTextBoxProgram);
            toolTip.Active = false;
        }

        /// <summary>
        /// Disable tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void richTextBoxProgram_MouseMove(object sender, MouseEventArgs e)
        {
            if ((toolTip != null) && (assembler != null))
            {
                int x = e.Location.X;
                int y = e.Location.Y;

                int charIndex = richTextBoxProgram.GetCharIndexFromPosition(new Point(x, y));
                int lineIndex = richTextBoxProgram.GetLineFromCharIndex(charIndex);

                bool found = false;
                for (int index = 0; (index < assembler.RAMprogramLine.Length) && !found; index++)
                {
                    if (assembler.RAMprogramLine[index] == lineIndex)
                    {
                        found = true;
                        if (toolTip.GetToolTip(richTextBoxProgram) != index.ToString("X4")) 
                        {
                            toolTip.Show(index.ToString("X4"), richTextBoxProgram, -50, richTextBoxProgram.GetPositionFromCharIndex(charIndex).Y, 50000);
                        } 
                    }
                }
            }
        }

        private void richTextBoxProgram_VScroll(object sender, EventArgs e)
        {
            UpdateBreakPoint(lineBreakPoint);
            toolTip.Hide(richTextBoxProgram);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updating the Registers
        /// </summary>
        private void UpdateRegisters()
        {
            if (assembler != null)
            {
                labelARegister.Text = assembler.registerA.ToString("X").PadLeft(2, '0');

                labelXRegister.Text = assembler.registerX.ToString("X").PadLeft(2, '0');
                labelYRegister.Text = assembler.registerY.ToString("X").PadLeft(2, '0');

                labelPCRegister.Text = assembler.registerPC.ToString("X").PadLeft(4, '0');
                labelSPRegister.Text = (0x0100 + assembler.registerSP).ToString("X").PadLeft(4, '0');
            } else
            {
                labelARegister.Text = "00";

                labelXRegister.Text = "00";
                labelYRegister.Text = "00";

                labelPCRegister.Text = "0000";
                labelSPRegister.Text = "0100";
            }
        }

        /// <summary>
        /// Update the Flags
        /// </summary>
        private void UpdateFlags()
        {
            if (assembler != null)
            {
                chkFlagC.Checked  = assembler.flagC;
                chkFlagV.Checked  = assembler.flagV;
                chkFlagD.Checked = assembler.flagD;
                chkFlagB.Checked  = assembler.flagB;
                chkFlag1.Checked  = assembler.flag1;
                chkFlagZ.Checked  = assembler.flagZ;
                chkFlagN.Checked  = assembler.flagN;
            } else
            {
                chkFlagC.Checked  = false;
                chkFlagV.Checked = false;
                chkFlagD.Checked = false;
                chkFlagB.Checked  = false;
                chkFlag1.Checked = true;
                chkFlagZ.Checked  = false;
                chkFlagN.Checked  = false;
            }
        }

        /// <summary>
        /// Draw memory panel starting from address startAddress, show nextAddress in green
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="nextAddress"></param>
        private void UpdateMemoryPanel(UInt16 startAddress, UInt16 nextAddress)
        {
            if (assembler != null)
            {
                // Boundary at address XXX0
                startAddress = (UInt16)(startAddress & 0xFFF0);

                // Check for overflow in display (startaddress + 0xFF larger then 0xFFFF)
                if (startAddress > 0xFF00) startAddress = 0xFF00;

                int i = startAddress;
                int j = 0;

                foreach (Label lbl in memoryAddressLabels)
                {
                    lbl.Text = i.ToString("X").PadLeft(4, '0');
                    i += 0x10;
                }

                i = 0;
                j = 0;

                // MemoryTableLabels, display the memory contents
                foreach (Label lbl in memoryTableLabels)
                {
                    int address = startAddress + (16 * i) + j;
                    lbl.Text = assembler.RAM[address].ToString("X").PadLeft(2, '0');

                    if (address == nextAddress)
                    {
                        lbl.BackColor = Color.LightGreen;
                    } else
                    if (address == 0x0100 + assembler.registerSP)
                    {
                        lbl.BackColor = Color.LightPink;
                    } else
                    {
                        lbl.BackColor = SystemColors.Info;
                    }

                    j++;
                    if (j == 0x10)
                    {
                        j = 0;
                        i++;
                    }
                }
            } else
            {
                int i = 0;

                foreach (Label lbl in memoryAddressLabels)
                {
                    lbl.Text = i.ToString("X").PadLeft(4, '0');
                    i += 0x10;
                }

                // MemoryTableLabels, display 00
                foreach (Label lbl in memoryTableLabels)
                {
                    lbl.Text = "00";
                    lbl.BackColor = SystemColors.Info;
                }
            }
        }

        /// <summary>
        /// Update 7 segment display of  ucJunior Computer (if active)
        /// </summary>
        private void ClearDisplay()
        {
            ucJunior.sevenSegmentData0.SegmentsValue = 0x00;
            ucJunior.sevenSegmentData1.SegmentsValue = 0x00;
            ucJunior.sevenSegmentAddress0.SegmentsValue = 0x00;
            ucJunior.sevenSegmentAddress1.SegmentsValue = 0x00;
            ucJunior.sevenSegmentAddress2.SegmentsValue = 0x00;
            ucJunior.sevenSegmentAddress3.SegmentsValue = 0x00;
        }

        /// <summary>
        /// Update 7 segment display of  ucJunior Computer
        /// </summary>
        private void UpdateDisplay()
        {
            if (assembler != null)
            {
                int value = 0x00;

                if (!chkLatched.Checked)
                {
                    ucJunior.sevenSegmentAddress3.SegmentsValue = value;
                    ucJunior.sevenSegmentAddress2.SegmentsValue = value;
                    ucJunior.sevenSegmentAddress1.SegmentsValue = value;
                    ucJunior.sevenSegmentAddress0.SegmentsValue = value;
                    ucJunior.sevenSegmentData1.SegmentsValue = value;
                    ucJunior.sevenSegmentData0.SegmentsValue = value;
                }

                if (((assembler.RAM[0x1A81] & 0b00000001) == 0b00000001) && ((assembler.RAM[0x1A80] & 0b00000001) == 0b00000001)) value |= 0x01;
                if (((assembler.RAM[0x1A81] & 0b00000010) == 0b00000010) && ((assembler.RAM[0x1A80] & 0b00000010) == 0b00000010)) value |= 0x02;
                if (((assembler.RAM[0x1A81] & 0b00000100) == 0b00000100) && ((assembler.RAM[0x1A80] & 0b00000100) == 0b00000100)) value |= 0x04;
                if (((assembler.RAM[0x1A81] & 0b00001000) == 0b00001000) && ((assembler.RAM[0x1A80] & 0b00001000) == 0b00001000)) value |= 0x08;
                if (((assembler.RAM[0x1A81] & 0b00010000) == 0b00010000) && ((assembler.RAM[0x1A80] & 0b00010000) == 0b00010000)) value |= 0x10;
                if (((assembler.RAM[0x1A81] & 0b00100000) == 0b00100000) && ((assembler.RAM[0x1A80] & 0b00100000) == 0b00100000)) value |= 0x20;
                if (((assembler.RAM[0x1A81] & 0b01000000) == 0b01000000) && ((assembler.RAM[0x1A80] & 0b01000000) == 0b01000000)) value |= 0x40;

                if (value != 0x7F || !chkLatched.Checked)
                {
                    // Invert value (active low)
                    value = 0x7F - value;

                    if (((assembler.RAM[0x1A83] & 0b00001000) == 0b00001000) && (assembler.RAM[0x1A82] == 0b00001000)) ucJunior.sevenSegmentAddress3.SegmentsValue = value;
                    if (((assembler.RAM[0x1A83] & 0b00001010) == 0b00001010) && (assembler.RAM[0x1A82] == 0b00001010)) ucJunior.sevenSegmentAddress2.SegmentsValue = value;
                    if (((assembler.RAM[0x1A83] & 0b00001100) == 0b00001100) && (assembler.RAM[0x1A82] == 0b00001100)) ucJunior.sevenSegmentAddress1.SegmentsValue = value;
                    if (((assembler.RAM[0x1A83] & 0b00001110) == 0b00001110) && (assembler.RAM[0x1A82] == 0b00001110)) ucJunior.sevenSegmentAddress0.SegmentsValue = value;
                    if (((assembler.RAM[0x1A83] & 0b00010000) == 0b00010000) && (assembler.RAM[0x1A82] == 0b00010000)) ucJunior.sevenSegmentData1.SegmentsValue = value;
                    if (((assembler.RAM[0x1A83] & 0b00010010) == 0b00010010) && (assembler.RAM[0x1A82] == 0b00010010)) ucJunior.sevenSegmentData0.SegmentsValue = value;
                }
            }
        }

        /// <summary>
        /// Update keyboard of  ucJunior Computer (if active)
        /// </summary>
        private void UpdateKeyboard()
        {
            // Check for  ucJunior Computer active
            if (assembler != null)
            {
                // Fill keycode provided by the 6532 PIA
                if ((assembler.RAM[0x1A81] & 0b10000000) == 0b00000000) assembler.RAM[0x1A80] |= 0b10000000;
                if ((assembler.RAM[0x1A81] & 0b01000000) == 0b00000000) assembler.RAM[0x1A80] |= 0b01000000;
                if ((assembler.RAM[0x1A81] & 0b00100000) == 0b00000000) assembler.RAM[0x1A80] |= 0b00100000;
                if ((assembler.RAM[0x1A81] & 0b00010000) == 0b00000000) assembler.RAM[0x1A80] |= 0b00010000;
                if ((assembler.RAM[0x1A81] & 0b00001000) == 0b00000000) assembler.RAM[0x1A80] |= 0b00001000;
                if ((assembler.RAM[0x1A81] & 0b00000100) == 0b00000000) assembler.RAM[0x1A80] |= 0b00000100;
                if ((assembler.RAM[0x1A81] & 0b00000010) == 0b00000000) assembler.RAM[0x1A80] |= 0b00000010;
                if ((assembler.RAM[0x1A81] & 0b00000001) == 0b00000000) assembler.RAM[0x1A80] |= 0b00000001;

                if (ucJunior.key0.Pressed  && ((assembler.RAM[0x1A81] & 0b01000000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b10111111; }
                if (ucJunior.key1.Pressed  && ((assembler.RAM[0x1A81] & 0b00100000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11011111; }
                if (ucJunior.key2.Pressed  && ((assembler.RAM[0x1A81] & 0b00010000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11101111; }
                if (ucJunior.key3.Pressed  && ((assembler.RAM[0x1A81] & 0b00001000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11110111; }
                if (ucJunior.key4.Pressed  && ((assembler.RAM[0x1A81] & 0b00000100) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11111011; }
                if (ucJunior.key5.Pressed  && ((assembler.RAM[0x1A81] & 0b00000010) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11111101; }
                if (ucJunior.key6.Pressed  && ((assembler.RAM[0x1A81] & 0b00000001) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000000)) { assembler.RAM[0x1A80] = 0b11111110; }
                if (ucJunior.key7.Pressed  && ((assembler.RAM[0x1A81] & 0b01000000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b10111111; }
                if (ucJunior.key8.Pressed  && ((assembler.RAM[0x1A81] & 0b00100000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11011111; }
                if (ucJunior.key9.Pressed  && ((assembler.RAM[0x1A81] & 0b00010000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11101111; }
                if (ucJunior.keyA.Pressed  && ((assembler.RAM[0x1A81] & 0b00001000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11110111; }
                if (ucJunior.keyB.Pressed  && ((assembler.RAM[0x1A81] & 0b00000100) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11111011; }
                if (ucJunior.keyC.Pressed  && ((assembler.RAM[0x1A81] & 0b00000010) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11111101; }
                if (ucJunior.keyD.Pressed  && ((assembler.RAM[0x1A81] & 0b00000001) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000010)) { assembler.RAM[0x1A80] = 0b11111110; }
                if (ucJunior.keyE.Pressed  && ((assembler.RAM[0x1A81] & 0b01000000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b10111111; }
                if (ucJunior.keyF.Pressed  && ((assembler.RAM[0x1A81] & 0b00100000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11011111; }
                if (ucJunior.keyAD.Pressed && ((assembler.RAM[0x1A81] & 0b00010000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11101111; }
                if (ucJunior.keyDA.Pressed && ((assembler.RAM[0x1A81] & 0b00001000) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11110111; }
                if (ucJunior.keyPL.Pressed && ((assembler.RAM[0x1A81] & 0b00000100) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11111011; }
                if (ucJunior.keyGO.Pressed && ((assembler.RAM[0x1A81] & 0b00000010) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11111101; }
                if (ucJunior.keyPC.Pressed && ((assembler.RAM[0x1A81] & 0b00000001) == 0b00000000) && ((assembler.RAM[0x1A82] & 0b00011110) == 0b00000100)) { assembler.RAM[0x1A80] = 0b11111110; }

                if (ucJunior.keyNMI.Pressed) 
                {
                    NMI();
                    ucJunior.keyNMI.Pressed = false;
                }

                if (ucJunior.keyRESET.Pressed) 
                {
                    if (!chkFlagI.Checked)
                    {
                        RESET();
                        ucJunior.keyRESET.Pressed = false;
                    }
                }
            }
        }

        /// <summary>
        /// Execute NMI routine
        /// </summary>
        private void NMI()
        {
            int address = assembler.RAM[0x1FFA] + 0x100 * assembler.RAM[0x1FFB];
            tbMemoryStartAddress.Text = address.ToString("X4");
            tbSetProgramCounter.Text = address.ToString("X4");
            assembler.registerPC = (UInt16)address;
            nextInstrAddress = (UInt16)address;
        }

        /// <summary>
        /// Execute RESET routine
        /// </summary>
        private void RESET()
        {
            int address = assembler.RAM[0x1FFC] + 0x100 * assembler.RAM[0x1FFD];
            tbMemoryStartAddress.Text = address.ToString("X4");
            tbSetProgramCounter.Text = address.ToString("X4");
            assembler.registerPC = (UInt16)address;
            nextInstrAddress = (UInt16)address;
        }

        /// <summary>
        /// get the memory start address from text box
        /// </summary>
        /// <returns></returns>
        private UInt16 GetTextBoxMemoryStartAddress()
        {
            string txtval = tbMemoryStartAddress.Text;
            UInt16 n = Convert.ToUInt16(txtval, 16);    // convert HEX to INT
            return n;
        }

        /// <summary>
        /// Change colors rich text box
        /// </summary>
        /// <param name="line_number"></param>
        /// <param name="error"></param>
        private void ChangeColorRTBLine(int line_number, bool error)
        {
            if ((line_number >= 0) && (richTextBoxProgram.Lines.Length > line_number))
            {
                // No layout events for now (postpone)
                richTextBoxProgram.SuspendLayout();

                // Disable certain event handlers completely
                richTextBoxProgram.TextChanged -= richTextBoxProgram_TextChanged;
                richTextBoxProgram.SelectionChanged -= richTextBoxProgram_SelectionChanged;

                // No focus so we won't see flicker from selection changes
                lblSetProgramCounter.Focus();

                // Reset color
                richTextBoxProgram.HideSelection = true;
                richTextBoxProgram.SelectAll();
                richTextBoxProgram.SelectionBackColor = System.Drawing.Color.White;
                richTextBoxProgram.DeselectAll();
                richTextBoxProgram.HideSelection = false;

                // Get location in RTB
                int firstcharindex = richTextBoxProgram.GetFirstCharIndexFromLine(line_number);
                string currentlinetext = richTextBoxProgram.Lines[line_number];

                // Select line and color red/green
                richTextBoxProgram.SelectionStart = firstcharindex;
                richTextBoxProgram.SelectionLength = currentlinetext.Length;
                richTextBoxProgram.SelectionBackColor = System.Drawing.Color.LightGreen;
                if (error) richTextBoxProgram.SelectionBackColor = System.Drawing.Color.LightPink;

                // Reset selection
                richTextBoxProgram.SelectionStart = firstcharindex;
                richTextBoxProgram.SelectionLength = 0;

                // Scroll to line (show 1 line before selected line if available)
                if (line_number != 0)
                {
                    firstcharindex = richTextBoxProgram.GetFirstCharIndexFromLine(line_number - 1);
                    richTextBoxProgram.SelectionStart = firstcharindex;
                }

                richTextBoxProgram.ScrollToCaret();

                // Set cursor at selected line
                firstcharindex = richTextBoxProgram.GetFirstCharIndexFromLine(line_number);
                richTextBoxProgram.SelectionStart = firstcharindex;
                richTextBoxProgram.SelectionLength = 0;

                // Set focus again
                richTextBoxProgram.Focus();

                // Enable event handler
                richTextBoxProgram.TextChanged += new EventHandler(richTextBoxProgram_TextChanged);
                richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);

                // Resume events 
                richTextBoxProgram.ResumeLayout();
            }
        }

        /// <summary>
        /// Clear colors rich text box
        /// </summary>
        private void ClearColorRTBLine()
        {
            // No layout events for now (postpone)
            richTextBoxProgram.SuspendLayout();

            // Disable certain event handlers completely
            richTextBoxProgram.TextChanged -= richTextBoxProgram_TextChanged;
            richTextBoxProgram.SelectionChanged -= richTextBoxProgram_SelectionChanged;

            // No focus so we won't see flicker from selection changes
            lblSetProgramCounter.Focus();

            // Reset color
            richTextBoxProgram.HideSelection = true;
            richTextBoxProgram.SelectAll();
            richTextBoxProgram.SelectionBackColor = System.Drawing.Color.White;
            richTextBoxProgram.DeselectAll();
            richTextBoxProgram.HideSelection = false;

            // Set focus again
            richTextBoxProgram.Focus();

            // Update breakpoint indicator
            UpdateBreakPoint(lineBreakPoint);

            // Enable event handler
            richTextBoxProgram.TextChanged += new EventHandler(richTextBoxProgram_TextChanged);
            richTextBoxProgram.SelectionChanged += new EventHandler(richTextBoxProgram_SelectionChanged);

            // Resume events 
            richTextBoxProgram.ResumeLayout();
        }

        /// <summary>
        /// show tooltip with string binaryval when we hover mouse over a (register) label 
        /// </summary>
        /// <param name="l"></param>
        private void RegisterHoverBinary(Label l)
        {
            string binaryval;
            binaryval = Convert.ToString(Convert.ToInt32(l.Text, 16), 2);

            // change the HEX string to BINARY string
            binaryval = binaryval.PadLeft(8, '0');
            toolTipRegisterBinary.SetToolTip(l, binaryval);
        }

        /// <summary>
        /// Update picturebox with breakpoint
        /// </summary>
        private void UpdateBreakPoint(int line)
        {
            // Clear other breakpoint
            Graphics g = pbBreakPoint.CreateGraphics();
            g.Clear(Color.LightGray);

            if (line >= 0)
            {
                int index = richTextBoxProgram.GetFirstCharIndexFromLine(line);
                if (index > 0)
                {
                    Point point = richTextBoxProgram.GetPositionFromCharIndex(index);
                    g.FillEllipse(Brushes.Red, new Rectangle(1, richTextBoxProgram.Margin.Top + point.Y, 15, 15));
                }
            }
        }

        /// <summary>
        /// Add info for each instruction button
        /// </summary>
        private void InitButtons()
        {
            // All instructions, sorted by name
            Instructions instructions = new Instructions();
            Array.Sort(instructions.MainInstructions, (x, y) => x.Mnemonic.CompareTo(y.Mnemonic));

            // Add all instruction buttons
            int numInstruction;

            // Main    
            numInstruction = 0;
            for (int indexInstructions = 0; indexInstructions < instructions.MainInstructions.Length; indexInstructions++)
            {
                Instruction instruction = instructions.MainInstructions[indexInstructions];

                if ((instruction.Mnemonic != "-") && (instruction.Size != 0))
                {
                    int x = 2 + (numInstruction % 4) * 68;
                    int y = 2 + (numInstruction / 4) * 25;

                    Button button = new Button();
                    button.BackColor = Color.LightGreen;
                    button.Location = new Point(x, y);
                    button.Name = "btn" + instruction.Opcode.ToString();
                    button.Size = new Size(66, 23);
                    button.Text = instruction.Mnemonic;
                    button.UseVisualStyleBackColor = false;
                    button.Click += new EventHandler(this.btnCommand_Click);
                    button.Tag = instruction;
                    button.MouseHover += new EventHandler(Control_MouseHover);

                    panelInstructions.Controls.Add(button);

                    numInstruction++;
                }
            }
        }

        #endregion
    }
}
