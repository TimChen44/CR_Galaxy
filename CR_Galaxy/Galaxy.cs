using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CR_Soft.ClassLibrary.Log;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CR_Galaxy
{
    public partial class Galaxy : Form
    {
        //  public bool _ScanningState;
        public Scanning _Scanning;
        private ScanningRankings _ScaRankings;
        //public string _EndUrl;
        private IOData _IOData;
        private Log Log;

        public Navigate _Navigate;
        private ServerInfo _SerInfo;//��������Ϣ

        private bool ControlLoad = false;//�Ƿ��һ�μ��أ��Ǿͳ�ʼ���ؼ�

        public WebBrowser MsgWB = new WebBrowser();//Ⱥ���ż����ͣ���ʱ
        public int UserIndex=0;
        public int MsgCount = 0;
        //������Ϣ
        //public string MsgStr = "http://uni8.ogame.cn.com/game/index.php?page=writemessages&session={0}&gesendet=1&messageziel={1}&to={2}&betreff={3}&text={4}";
        public bool MsgCancel = false;

        public string _Session;

        //�������ģ��
        public OGControl.OGControlManage _OGControlManage = new CR_Galaxy.OGControl.OGControlManage();
        //���ӿ���ģ�飬����ͳ����ƽ���
       public OGControl.FlottenCommand _FlottenCmd = new CR_Galaxy.OGControl.FlottenCommand();

        public Galaxy()
        {
            InitializeComponent();
            MsgWB.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(MsgWB_DocumentCompleted);
        }

        //�������������
        void MsgWB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //MsgCount++;
            //label9.Text = "�Ѿ�����" + MsgCount.ToString();
            //UserIndex++;
            //if (UserIndex >= userList.Items.Count) UserIndex = 0;
            //Random ro=new Random(unchecked((int)DateTime.Now.Ticks));

            //TxtLog.Text += "\r\n" + string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "�ޱ���", msgList.Lines[ro.Next(msgList.Lines.Length)] });
            //if (MsgCancel==false)
            //    MsgWB.Navigate(string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "�ޱ���", msgList.Lines[ro.Next(msgList.Lines.Length)] }));
        
        
        }

        private void Galaxy_Load(object sender, EventArgs e)
        {


            Log = new Log(TxtLog);
            _SerInfo = new ServerInfo();
            _Navigate = new Navigate();
            _IOData = new IOData(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Date.mdb", Log);
            _Scanning = new Scanning(GS_Web, _IOData, Log, _SerInfo);
            _ScaRankings = new ScanningRankings(_Navigate, _SerInfo, GS_Web, _IOData, Log);

            GridMain.AutoGenerateColumns = false;//���Զ������
            GridPlanet.AutoGenerateColumns = false;
            GridNeighbors.AutoGenerateColumns = false;
            GridUserEes.AutoGenerateColumns = false;
            GridUserFlt.AutoGenerateColumns = false;
            GridUserPts.AutoGenerateColumns = false;
            GridUnionEes.AutoGenerateColumns = false;
            GridUnionFlt.AutoGenerateColumns = false;
            GridUnionPts.AutoGenerateColumns = false;

            //���ò�������
            _IOData.SetRelationsRead(SetFriendList, SetEnemyList, SetUnionList, SetEnemyUnionList);
            DataTable DT = _IOData.SetMy();

            SetMyUnion.Text = DT.Rows[0]["My"].ToString();

            ShowMyUnion.Checked = (bool)DT.Rows[0]["MyUnion"];
            ShowFriend.Checked = (bool)DT.Rows[0]["Friend"];
            ShowEnemy.Checked = (bool)DT.Rows[0]["Enemy"];
            ShowUnion.Checked = (bool)DT.Rows[0]["Union"];
            ShowEnemyUnion.Checked = (bool)DT.Rows[0]["EnemyUnion"];
            ShowiL.Checked = (bool)DT.Rows[0]["i"];
            ShowV.Checked = (bool)DT.Rows[0]["u"];
            ShowG.Checked = (bool)DT.Rows[0]["g"];

            //���ð�ť�ı���ɫ
            SetMyUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["MyUnionColor"]);

            SetFriendColor.BackColor = Color.FromArgb((int)DT.Rows[0]["FriendColor"]);
            SetEnemyColor.BackColor = Color.FromArgb((int)DT.Rows[0]["EnemyColor"]);
            SetUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["UnionColor"]);
            SetEnemyUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["EnemyUnionColor"]);
            SetLiColor.BackColor = Color.FromArgb((int)DT.Rows[0]["iColor"]);
            SetuColor.BackColor = Color.FromArgb((int)DT.Rows[0]["uColor"]);
            SetgColor.BackColor = Color.FromArgb((int)DT.Rows[0]["gColor"]);
            SetGridBack.BackColor = Color.FromArgb((int)DT.Rows[0]["GridBack"]);
            SetGridFore.BackColor = Color.FromArgb((int)DT.Rows[0]["SetGridFore"]);
            SetCustom1Color.BackColor = Color.FromArgb((int)DT.Rows[0]["Custom1Color"]);
            SetCustom2Color.BackColor = Color.FromArgb((int)DT.Rows[0]["Custom2Color"]);

            //���ñ���ɫ��ǰ��ɫ
            GridMain.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridPlanet.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridNeighbors.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridMain.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridPlanet.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridNeighbors.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;

            GridUserEes.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUserFlt.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUserPts.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionEes.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionFlt.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionPts.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;

            GridUserEes.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUserFlt.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUserPts.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionEes.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionFlt.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionPts.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            //����
        }

        private void Galaxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IOData.SetWrite(SetFriendList, SetEnemyList, SetUnionList, SetEnemyUnionList, new string[] { 
            SetMyUnion.Text,
            ShowMyUnion.Checked.ToString(),
            ShowFriend.Checked.ToString(),
            ShowEnemy.Checked.ToString(),
            ShowUnion.Checked.ToString(),
            ShowEnemyUnion.Checked.ToString(),
            ShowiL.Checked.ToString(),
            ShowV.Checked.ToString(),
            ShowG.Checked.ToString(),
            SetMyUnionColor.BackColor.ToArgb().ToString(),
            SetFriendColor.BackColor.ToArgb().ToString(),
            SetEnemyColor.BackColor.ToArgb().ToString(),
            SetUnionColor.BackColor.ToArgb().ToString(),
            SetEnemyUnionColor.BackColor.ToArgb().ToString(),
            SetLiColor.BackColor.ToArgb().ToString(),
            SetuColor.BackColor.ToArgb().ToString(),
            SetgColor.BackColor.ToArgb().ToString(),
            SetGridBack.BackColor.ToArgb().ToString(),
            SetGridFore.BackColor.ToArgb().ToString(),
            SetCustom1Color.BackColor.ToArgb().ToString(),
            SetCustom2Color.BackColor.ToArgb().ToString()
            });
        }

        private void GS_WebGo_Click(object sender, EventArgs e)
        {
            GS_WebAddress_SelectedIndexChanged(sender, null);
            // GS_Web.Navigate(GS_WebAddress.Text);
        }

        private void GS_WebRetreat_Click(object sender, EventArgs e)
        {
            GS_Web.GoBack();
        }

        private void GS_WebForward_Click(object sender, EventArgs e)
        {
            GS_Web.GoForward();
        }

        private void GS_WebStop_Click(object sender, EventArgs e)
        {
            GS_Web.Stop();
        }

        private void GS_WebRefresh_Click(object sender, EventArgs e)
        {
            GS_Web.Refresh();
        }

        private void GS_Web_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            GS_WebProBar.Maximum = Convert.ToInt32(e.MaximumProgress);
            GS_WebProBar.Value = Convert.ToInt32(e.CurrentProgress);
        }

        private void GalaxyScanning_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 1;
                _Scanning.BeginScanning(Convert.ToInt32(GS_WebBeginGalaxy.Text), Convert.ToInt32(GS_WebBeginSystem.Text),
                Convert.ToInt32(GS_WebEndGalaxy.Text), Convert.ToInt32(GS_WebEndSystem.Text), _Navigate);

                GS_ScanningProBar.Maximum = _Scanning.GetScanningCount();//������ɨ������
                GS_SysValue.Text = "0";
                GS_SysMax.Text = GS_ScanningProBar.Maximum.ToString();//������ɨ������

                State.Text = "״̬����ͼɨ��";
                GalaxyScanning.Text = "ɨ����...(���ֹͣ)";
                IEgo.Enabled = false;
            }
            else if (_Navigate.Work == 1)
            {
                _Navigate.Work = 0;
                State.Text = "״̬�����ɲ���";
                GalaxyScanning.Text = "��ʼɨ��";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("��һ���������ڽ����У�����ֹͣ��������");

            }
        }

        /// <summary>
        /// �����ʱģ��
        /// </summary>
        private void SleepTime()
        {
            Random Rm = new Random();
            int i = Rm.Next(Convert.ToInt32(GS_Time.Tag));
            Log.AddInformation("�����ʱ " + i.ToString() + " ��");
            Application.DoEvents();
            Thread.Sleep(i * 1000);
        }

        private void GS_Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _Navigate.State = 1;
            if (_Navigate.Work == 1)
            {
                SleepTime();

                State.Text = "״̬����ͼɨ��";
                try
                {
                    Log.AddInformation("��ʼɨ��" + _Scanning.GetNowSystem());
                    _Navigate.WorkState = _Scanning.Main();

                }
                catch (Exception eee)
                {
                    Log.AddError("<ɨ��>" + eee.Message);
                    Log.AddInformation("ɨ��ʧ��" + _Scanning.GetNowSystem());
                }

                if (_Scanning.GetScanningNumValue() <= GS_ScanningProBar.Maximum)//����������
                {
                    GS_ScanningProBar.Value = _Scanning.GetScanningNumValue();
                    GS_SysValue.Text = GS_ScanningProBar.Value.ToString();
                }

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("һ��ɨ����" + _Scanning.GetScanningNumValue().ToString() + "̫��ϵ");
                    GalaxyScanning.Text = "��ʼɨ��";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("ɨ�����");
                }
            }
            else if (_Navigate.Work == 2)//ɨ���������
            {
                SleepTime();

                State.Text = "״̬�����ɨ��";
                try
                {
                    //Thread.Sleep(new Random().Next(500,1000));
                    _Navigate.WorkState = _ScaRankings.MainPlayer();
                    Log.AddInformation("�Ѿ�ɨ��" + _ScaRankings.GetPlayerStart() + " - " + _ScaRankings.GetSacCount());
                }
                catch (Exception eee)
                {
                    Log.AddError("<ɨ��>" + eee.Message);
                    Log.AddInformation("ɨ��ʧ��" + _ScaRankings.GetPlayerStart());
                }

                GS_SysValue.Text = Convert.ToString((Convert.ToInt32(GS_SysValue.Text) + 1));
                GS_SysMax.Text = Convert.ToString((Convert.ToInt32(GS_SysMax.Text) + 1));

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("һ��ɨ����" + _ScaRankings.GetSacCount() + "��");
                    ScanningPlayer.Text = "ɨ���������";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("ɨ�����");
                }


            }
            else if (_Navigate.Work == 3)//ɨ�轢������
            {
                SleepTime();

                State.Text = "״̬������ɨ��";

                try
                {
                    //Thread.Sleep(new Random().Next(500,1000));
                    _Navigate.WorkState = _ScaRankings.MainUnion();
                    Log.AddInformation("�Ѿ�ɨ��" + _ScaRankings.GetPlayerStart() + " - " + _ScaRankings.GetSacCount());
                }
                catch (Exception eee)
                {
                    Log.AddError("<ɨ��>" + eee.Message);
                    Log.AddInformation("ɨ��ʧ��" + _ScaRankings.GetPlayerStart());
                }

                GS_SysValue.Text = _ScaRankings.GetSacCount().ToString();
                GS_SysMax.Text = GS_SysValue.Text;

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("һ��ɨ����" + _ScaRankings.GetSacCount() + "��");
                    ScanningUnion.Text = "ɨ�轢������";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("ɨ�����");
                }


            }
            else if (_Navigate.Work == 10)
            {
                State.Text = "״̬��������";
                bool TmpSes;
                TmpSes = _Navigate.SaveMainSession(GS_Web.Document);
                if (TmpSes)
                {
                    _Navigate.Work = 0;
                    State.Text = "״̬�����ɲ���";
                }
            }
            else if (_Navigate.Work == 0)
            {
                State.Text = "״̬�����ɲ���";
            }

            _Navigate.State = 0;

        }

        private void GS_Web_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            _Navigate.State = 1;
        }

        private void GS_Web_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            _Navigate.EndUrl = e.Url.OriginalString.ToString();
        }

        private void ɨ��ȫ������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GS_WebBeginGalaxy.Text = "1";
            GS_WebBeginSystem.Text = "1";
            GS_WebEndGalaxy.Text = "9";
            GS_WebEndSystem.Text = "499";
            GalaxyScanning_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            GS_WebBeginGalaxy.Text = ((ToolStripMenuItem)sender).Text;
            GS_WebBeginSystem.Text = "1";
            GS_WebEndGalaxy.Text = ((ToolStripMenuItem)sender).Text;
            GS_WebEndSystem.Text = "499";
            GalaxyScanning_Click(sender, e);
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly cc = Assembly.GetExecutingAssembly();
            about1.MyAssembly = cc;
            about1.ShowDialog();
        }

        private void ����֧��ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://blog.163.com/cr_soft/");

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.ShowAll();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastPlanet(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastUser(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastUnion(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void M_MHFast_Click(object sender, EventArgs e)
        {
            M_MHFast.Checked = !M_MHFast.Checked;
        }

        private void ToolTxtSelectAll(object sender, MouseEventArgs e)
        {
            ((ToolStripTextBox)sender).SelectAll();
        }

        private void toolStripComboBox5_MouseDown(object sender, MouseEventArgs e)
        {
            M_FUserName.SelectAll();
        }

        private void toolStripComboBox7_MouseDown(object sender, MouseEventArgs e)
        {
            M_FUnion.SelectAll();
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastGalaxy(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void ����֧��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�ȳ�����������д^_^");
        }

        private void yToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.Findi(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindI(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void GridPlanet_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               // GridNeighbors.DataSource = _IOData.FindPlanetNeighbors(GridPlanet.SelectedRows[0].Cells["PGalaxy"].Value.ToString(), GridPlanet.SelectedRows[0].Cells["PSystem"].Value.ToString());
                GridNeighbors.DataSource = _IOData.FindPlanetNeighbors(GridPlanet["PGalaxy", e.RowIndex].Value.ToString(), GridPlanet["PSystem", e.RowIndex].Value.ToString());
                GroupNeighbors.Text = "��ϵ: " + GridPlanet["PGalaxySystem", e.RowIndex].Value.ToString();
            }
            catch (Exception ee)
            {
                Log.AddError("<GridPlanet_RowEnter>" + ee.Message);
                GridNeighbors.DataSource = null;
            }
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_Galaxy.Text) > 1)
            {
                M_Galaxy.Text = (Convert.ToInt32(M_Galaxy.Text) - 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_Galaxy.Text) < 9)
            {
                M_Galaxy.Text = (Convert.ToInt32(M_Galaxy.Text) + 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_System.Text) > 1)
            {
                M_System.Text = (Convert.ToInt32(M_System.Text) - 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_System.Text) < 499)
            {
                M_System.Text = (Convert.ToInt32(M_System.Text) + 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void UserRankings(object sender, EventArgs e)
        {
            string Ran = ((ToolStripMenuItem)sender).Text;
            GridMain.DataSource = _IOData.FindUserRankings(Ran.Substring(0, Ran.IndexOf("~")).Trim(), Ran.Substring(Ran.IndexOf("~") + 1).Trim());
        }

        private void UnionRankings_Click(object sender, EventArgs e)
        {
            string Ran = ((ToolStripMenuItem)sender).Text;
            GridMain.DataSource = _IOData.FindUserUnionRankings(Ran.Substring(0, Ran.IndexOf("~")).Trim(), Ran.Substring(Ran.IndexOf("~") + 1).Trim());
        }

        private void M_FFind_Click(object sender, EventArgs e)
        {
            string Galaxy, UserName, Rankings, Union, UnionRankings, i, Li, Vacation, Banned;

            //��ϵ
            if ((M_FSys.Text == "") || (M_FSys.Text == "ȫ��"))
                Galaxy = "1 = 1";
            else
                Galaxy = "[Galaxy] = " + M_FSys.Text.Substring(0, 1).Trim();

            //�û���
            if ((M_FUserName.Text == "") || (M_FUserName.Text == "�����û���"))
                UserName = "1=1";
            else
                UserName = "[UserName] = '" + M_FUserName.Text + "'";

            //����
            if ((M_FRankings.Text == "") || (M_FRankings.Text == "ȫ��"))
                Rankings = "1=1";
            else
            {
                if (M_FRankings.SelectedIndex == 1)
                    Rankings = "[Rankings] >= 1 and [Rankings] <= 99";
                else
                    Rankings = "[Rankings] >= " + Convert.ToString(M_FRankings.SelectedIndex * 100 - 100) + " and [Rankings] <= " + Convert.ToString(M_FRankings.SelectedIndex * 100 - 1);
            }

            //����
            if ((M_FUnion.Text == "") || (M_FUnion.Text == "����������"))
                Union = "1=1";
            else
                Union = "[Union] = '" + M_FUnion.Text + "'";

            //��������
            if ((M_FUnionRankings.Text == "") || (M_FUnionRankings.Text == "ȫ��"))
                UnionRankings = "1=1";
            else
            {
                if (M_FUnionRankings.SelectedIndex == 1)
                    UnionRankings = "[UnionRankings] >= 1 and [UnionRankings] <= 99";
                else
                    UnionRankings = "[UnionRankings] >= " + Convert.ToString(M_FUnionRankings.SelectedIndex * 100 - 100) + " and [UnionRankings] <= " + Convert.ToString(M_FUnionRankings.SelectedIndex * 100 - 1);
            }

            //7�첻����
            if (M_Fi.Text == "��")
                i = "[inactive] = false";
            else if (M_Fi.Text == "��")
                i = "[inactive] = true";
            else
                i = "1=1";

            //28�첻����
            if (M_FLi.Text == "��")
                Li = "[longinactive] = false";
            else if (M_FLi.Text == "��")
                Li = "[longinactive] = true";
            else
                Li = "1=1";

            //����
            if (M_FVacation.Text == "��")
                Vacation = "[vacation] = false";
            else if (M_FVacation.Text == "��")
                Vacation = "[vacation] = true";
            else
                Vacation = "1=1";

            //����
            if (M_FBanned.Text == "��")
                Banned = "[banned] = false";
            else if (M_FBanned.Text == "��")
                Banned = "[banned] = true";
            else
                Banned = "1=1";

            GridMain.DataSource = _IOData.FindAdvancedFind(Galaxy, UserName, Rankings, Union, UnionRankings, i, Li, Vacation, Banned);

        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " ALL.XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutAllData(saveFileDialog1.FileName);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + ".XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutNowData(saveFileDialog1.FileName);
        }

        private void SaveGalaxyAllData_Click(object sender, EventArgs e)
        {
            string LGaxlax = ((ToolStripMenuItem)sender).Text;
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " " + LGaxlax + " ALL.XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutGalaxyAllData(saveFileDialog1.FileName, LGaxlax.Substring(0, 1));
        }

        private void SaveGalaxyNowData_Click(object sender, EventArgs e)
        {
            string LGaxlax = ((ToolStripMenuItem)sender).Text;
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " " + LGaxlax + ".XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutGalaxyNowData(saveFileDialog1.FileName, LGaxlax.Substring(0, 1));
        }

        private void toolStripButton16_Click_1(object sender, EventArgs e)
        {

            //openFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\ ";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataInBox.Left = DataInBox.Parent.Width / 2 - 200;
                DataInBox.Top = DataInBox.Parent.Height / 2 - 20;
                DataInBox.Visible = true;
                tabPage2.Enabled = false;
                _IOData.InData(openFileDialog1.FileName, DataInBar);
                tabPage2.Enabled = true;
                DataInBox.Visible = false;

                Log.AddInformation("���ݵ������");
            }
        }

        private void ���汾����Ԥ�������кܶ๦��δ������Щ���ܽ��ڽ��ڲ�ȫToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://blog.163.com/cr_soft/");
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            if (FlyTimeWeb.Url.ToString() == "about:blank")
                FlyTimeWeb.Navigate(Path.GetDirectoryName(Application.ExecutablePath) + "\\Tool\\Time.htm");
        }

        private void ExRowPostPaint(DataGridViewRow dgr)
        {
            int i;

            if (dgr.Cells["Custom1"].Value.ToString().Trim() == "True")
            {
                dgr.DefaultCellStyle.BackColor = SetCustom1Color.BackColor;
                return;
            }
            //else
            //{
            //    dgr.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            //}

            if (dgr.Cells["Union"].Value != null)
            {
                //�Լ�����
                if ((ShowMyUnion.Checked) && (SetMyUnion.Text != ""))
                    if (dgr.Cells["Union"].Value.ToString().Trim() == SetMyUnion.Text)
                    {
                        dgr.DefaultCellStyle.BackColor = SetMyUnionColor.BackColor;
                        return;
                    }

                //����
                if (ShowUnion.Checked)
                    for (i = 0; i < SetUnionList.Items.Count; i++)
                        if (dgr.Cells["Union"].Value.ToString().Trim() == SetUnionList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetUnionColor.BackColor;
                            return;
                        }

                //�ж�����
                if (ShowEnemyUnion.Checked)
                    for (i = 0; i < SetEnemyUnionList.Items.Count; i++)
                        if (dgr.Cells["Union"].Value.ToString().Trim() == SetEnemyUnionList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetEnemyUnionColor.BackColor;
                            return;
                        }
            }

            if (dgr.Cells["UserName"].Value != null)
            {
                //����
                if (ShowFriend.Checked)
                    for (i = 0; i < SetFriendList.Items.Count; i++)
                        if (dgr.Cells["UserName"].Value.ToString().Trim() == SetFriendList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetFriendColor.BackColor;
                            return;
                        }

                //����
                if (ShowEnemy.Checked)
                    for (i = 0; i < SetEnemyList.Items.Count; i++)
                        if (dgr.Cells["UserName"].Value.ToString().Trim() == SetEnemyList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetEnemyColor.BackColor;
                            return;
                        }
            }
        }

        private void GridMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private void GridMain_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= GridMain.Rows.Count - 1)
                return;
            DataGridViewRow dgr = GridMain.Rows[e.RowIndex];
            //���û����ҾͲ���ʾ������������ȼ�
            if (dgr.Cells["Username"].Value.ToString().Trim() == "")
            {
                dgr.DefaultCellStyle.ForeColor = dgr.DefaultCellStyle.BackColor;
                //dgr.DefaultCellStyle.SelectionForeColor  =dgr.DefaultCellStyle.SelectionBackColor  ;
                dgr.Cells[0].Style.ForeColor = Color.White;
                //dgr.Cells[0].Style.SelectionForeColor = Color.White;
                return;
            }
            try
            {
                ExRowPostPaint(dgr);//������ɫ�������������ñ�����

                //�������

                //i/I��
                //����
                //����
                if (dgr.Cells["Custom2"].Value.ToString().Trim() == "True")
                {
                    dgr.DefaultCellStyle.ForeColor = SetCustom2Color.BackColor;
                }
                if (dgr.Cells["banned"].Value.ToString() == "True")
                {
                    if (ShowG.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetgColor.BackColor;
                }
                else if (dgr.Cells["Vacation"].Value.ToString() == "True")
                {
                    if (ShowV.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetuColor.BackColor;
                }
                else if (dgr.Cells["inactive"].Value.ToString() == "True")
                {
                    dgr.DefaultCellStyle.ForeColor = SetLiColor.BackColor;
                }
                else if (dgr.Cells["longinactive"].Value.ToString() == "True")
                {
                    if (ShowiL.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetLiColor.BackColor;
                }
                else if (dgr.Cells["Custom2"].Value.ToString().Trim() == "False")
                {
                    dgr.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                }
            }
            catch (Exception ee)
            {
                Log.AddError("<GridMain_RowPostPaint>" + ee.Message);
            }
        }

        private void SetFriendAdd_Click(object sender, EventArgs e)
        {
            if (SetFriendTxt.Text != "������������")
            {
                SetFriendList.Items.Add(SetFriendTxt.Text);
            }
        }

        private void SetEnemyAdd_Click(object sender, EventArgs e)
        {
            if (SetEnemyTxt.Text != "�����������")
            {
                SetEnemyList.Items.Add(SetEnemyTxt.Text);
            }
        }

        private void SetUnionAdd_Click(object sender, EventArgs e)
        {
            if (SetUnionTxt.Text != "������������")
            {
                SetUnionList.Items.Add(SetUnionTxt.Text);
            }
        }

        private void SetEnemyUnionAdd_Click(object sender, EventArgs e)
        {
            if (SetEnemyUnionTxt.Text != "����ж���������")
            {
                SetEnemyUnionList.Items.Add(SetEnemyUnionTxt.Text);
            }
        }

        private void SetFriendDel_Click(object sender, EventArgs e)
        {
            if (SetFriendList.SelectedIndex != -1)
            {
                SetFriendList.Items.RemoveAt(SetFriendList.SelectedIndex);
            }
        }

        private void SetEnemyDel_Click_1(object sender, EventArgs e)
        {
            if (SetEnemyList.SelectedIndex != -1)
            {
                SetEnemyList.Items.RemoveAt(SetEnemyList.SelectedIndex);
            }
        }

        private void SetUnionDel_Click(object sender, EventArgs e)
        {
            if (SetUnionList.SelectedIndex != -1)
            {
                SetUnionList.Items.RemoveAt(SetUnionList.SelectedIndex);
            }
        }

        private void SetEnemyUnionDel_Click(object sender, EventArgs e)
        {
            if (SetEnemyUnionList.SelectedIndex != -1)
            {
                SetEnemyUnionList.Items.RemoveAt(SetEnemyUnionList.SelectedIndex);
            }
        }

        private void ShowMyUnion_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        //�����
        private void TabInfoClear()
        {
            GridUserEes.DataSource = null;
            GridUserFlt.DataSource = null;
            GridUserPts.DataSource = null;
            GridUnionEes.DataSource = null;
            GridUnionFlt.DataSource = null;
            GridUnionPts.DataSource = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
        }

        private void TxtSpy_Leave(object sender, EventArgs e)
        {
            if (GridMain["GalaxySystem", GridMain.CurrentCell.RowIndex].Value != null)
                _IOData.UpDataSpy(TxtSpy.Text, GridMain["GalaxySystem", GridMain.CurrentCell.RowIndex].Value.ToString());
        }

        private void SetMyUnionColor_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;

            // TxtLog.Text += ((Button)sender).BackColor.ToArgb().ToString() + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = Color1.Color;
                GridMain.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
                GridPlanet.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
                GridNeighbors.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
            }
        }

        private void SetGridFore_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = Color1.Color;
                GridMain.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                GridPlanet.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                GridNeighbors.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            }
        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void yToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.Find4Planet(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)), M_4Txt.Text);
        }

        private void GS_WebAddress_Click(object sender, EventArgs e)
        {

        }

        private void GS_WebAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GS_WebAddress.Text)
            {
                case "www.ogame.com.cn":
                    _SerInfo.CN();
                    ServerInfo.Text = "��������" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.com.cn");
                    break;
                case "www.ogame.tw":
                    _SerInfo.TW();
                    ServerInfo.Text = "��������" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.tw");
                    break;
                case "ogame.de":
                    _SerInfo.DE();
                    ServerInfo.Text = "��������" + _SerInfo.Server;
                    GS_Web.Navigate("ogame.de");
                    break;
                case "www.ogame.org":
                    _SerInfo.EN();
                    ServerInfo.Text = "��������" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.org");
                    break;
                default:
                    _SerInfo.CN();
                    ServerInfo.Text = "��������δ֪";
                    GS_Web.Navigate(GS_WebAddress.Text);
                    break;
            }
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            Login LG = new Login();
            LG.ShowDialog();
            if (LG.Result == DialogResult.OK)
            {
                _SerInfo.AutoServer(LG.Server);
                GS_WebAddress.Text = _SerInfo.Website;
                ServerInfo.Text = "��������" + _SerInfo.Server;
                _Navigate.Work = 10;
                GS_Web.Navigate(_SerInfo.LoginUrl(LG.U, LG.UserName, LG.UserPass));

            }
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton18_Click_1(object sender, EventArgs e)
        {
            string a = GS_Web.Document.Cookie.ToString();
        }

        private void ScanningPlayer_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 2;
                State.Text = "״̬�����ɨ��";
                ScanningPlayer.Text = "ɨ����...(���ֹͣ)";
                IEgo.Enabled = false;
                //GS_ScanningProBar.Maximum = 213;//������ɨ������
                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";//������ɨ������

                _ScaRankings.BeginPlayer();

            }
            else if (_Navigate.Work == 2)
            {
                _Navigate.Work = 0;
                State.Text = "״̬�����ɲ���";
                ScanningPlayer.Text = "ɨ���������";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("��һ���������ڽ����У�����ֹͣ��������");

            }

        }

        private void ScanningUnion_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 3;
                State.Text = "״̬������ɨ��";
                ScanningUnion.Text = "ɨ����...(���ֹͣ)";
                IEgo.Enabled = false;
                //GS_ScanningProBar.Maximum = 213;//������ɨ������
                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";//������ɨ������

                _ScaRankings.BeginUnion();

            }
            else if (_Navigate.Work == 3)
            {
                _Navigate.Work = 0;
                State.Text = "״̬�����ɲ���";
                ScanningUnion.Text = "ɨ�轢������";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("��һ���������ڽ����У�����ֹͣ��������");

            }
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage9_Enter(object sender, EventArgs e)
        {
            if (GridUserEes.DataSource == null)
            {
                GridUserEes.DataSource = _IOData.HistoryUserRankings("UserEes", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUserFlt.DataSource = _IOData.HistoryUserRankings("UserFlt", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUserPts.DataSource = _IOData.HistoryUserRankings("UserPts", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
            }
        }

        private void tabPage10_Enter(object sender, EventArgs e)
        {
            if (GridUnionEes.DataSource == null)
            {
                GridUnionEes.DataSource = _IOData.HistoryUnionRankings("UnionEes", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUnionFlt.DataSource = _IOData.HistoryUnionRankings("UnionFlt", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUnionPts.DataSource = _IOData.HistoryUnionRankings("UnionPts", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
            }
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {

        }

        private void yToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindNi(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.MetalCrystal();
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            _ScaRankings.SetScanningObject(GS_SB1.Checked, GS_SB2.Checked, GS_SB3.Checked);
        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {

        }

        private void GS_Time0_Click(object sender, EventArgs e)
        {
            GS_Time0.Checked = false;
            GS_Time1.Checked = false;
            GS_Time2.Checked = false;
            GS_Time3.Checked = false;
            GS_Time4.Checked = false;
            GS_Time5.Checked = false;
            GS_Time10.Checked = false;
            GS_Time15.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
            GS_Time.Tag = ((ToolStripMenuItem)sender).Text;
        }

        private void tabPage11_Enter(object sender, EventArgs e)
        {
            if (FightingReported.Url.ToString() == "about:blank")
                FightingReported.Navigate(Path.GetDirectoryName(Application.ExecutablePath) + "\\Tool\\Converter.htm");
        }

        private void SetCustom1Color_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;
        }

        private void SetCustom2Color_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;
        }

        private void toolStripButton11_Click_1(object sender, EventArgs e)
        {
            if (GridMain.SelectedRows != null)
            {
                bool Cus = Convert.ToBoolean(GridMain.SelectedRows[0].Cells["Custom1"].Value.ToString());
                _IOData.Custom1Check(GridMain.SelectedRows[0].Cells["GalaxySystem"].Value.ToString(), Cus);
                GridMain.SelectedRows[0].Cells["Custom1"].Value = !Cus;
            }
        }

        private void toolStripButton14_Click_1(object sender, EventArgs e)
        {
            if (GridMain.SelectedRows != null)
            {
                bool Cus = Convert.ToBoolean(GridMain.SelectedRows[0].Cells["Custom2"].Value.ToString());
                _IOData.Custom2Check(GridMain.SelectedRows[0].Cells["GalaxySystem"].Value.ToString(), Cus);
                GridMain.SelectedRows[0].Cells["Custom2"].Value = !Cus;
            }
        }

        private void tabpage13_Enter(object sender, EventArgs e)
        {

        }

        private void tabpage13_Leave(object sender, EventArgs e)
        {

        }

        private void tabPage12_Enter(object sender, EventArgs e)
        {
            resources1.LoadFile();
            resources2.LoadFile();
            resources3.LoadFile();
            resources4.LoadFile();
            resources5.LoadFile();
            resources6.LoadFile();
            resources7.LoadFile();
            resources8.LoadFile();
            resources9.LoadFile();
            Res();
        }

        private void tabPage12_Leave(object sender, EventArgs e)
        {
            resources1.SaveFile();
            resources2.SaveFile();
            resources3.SaveFile();
            resources4.SaveFile();
            resources5.SaveFile();
            resources6.SaveFile();
            resources7.SaveFile();
            resources8.SaveFile();
            resources9.SaveFile();
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        private void Res()
        {
            MetalAll.Text = Convert.ToString(resources1.GetMetalAll + resources2.GetMetalAll + resources3.GetMetalAll + resources4.GetMetalAll + resources5.GetMetalAll + resources6.GetMetalAll + resources7.GetMetalAll + resources8.GetMetalAll + resources9.GetMetalAll);
            MetalDay.Text = Convert.ToString(resources1.GetMetalDay + resources2.GetMetalDay + resources3.GetMetalDay + resources4.GetMetalDay + resources5.GetMetalDay + resources6.GetMetalDay + resources7.GetMetalDay + resources8.GetMetalDay + resources9.GetMetalDay);

            CrystalAll.Text = Convert.ToString(resources1.GetCrystalAll + resources2.GetCrystalAll + resources3.GetCrystalAll + resources4.GetCrystalAll + resources5.GetCrystalAll + resources6.GetCrystalAll + resources7.GetCrystalAll + resources8.GetCrystalAll + resources9.GetCrystalAll);
            CrystalDay.Text = Convert.ToString(resources1.GetCrystalDay + resources2.GetCrystalDay + resources3.GetCrystalDay + resources4.GetCrystalDay + resources5.GetCrystalDay + resources6.GetCrystalDay + resources7.GetCrystalDay + resources8.GetCrystalDay + resources9.GetCrystalDay);

            HHAll.Text = Convert.ToString(resources1.GetHHAll + resources2.GetHHAll + resources3.GetHHAll + resources4.GetHHAll + resources5.GetHHAll + resources6.GetHHAll + resources7.GetHHAll + resources8.GetHHAll + resources9.GetHHAll);
            HHDay.Text = Convert.ToString(resources1.GetHHDay + resources2.GetHHDay + resources3.GetHHDay + resources4.GetHHDay + resources5.GetHHDay + resources6.GetHHDay + resources7.GetHHDay + resources8.GetHHDay + resources9.GetHHDay);
        }

        private void resources1_btnOkClick(object sender, EventArgs e)
        {
            Res();
        }

        //private Match _PcMc;
        //private Match _PlTextMc;

        private void tabPage15_Enter(object sender, EventArgs e)
        {
            //����ȫ�ֿ��ƣ���ʼ���иó�ʼ����Դ
            if (GS_Web.Document == null) return;
            if (ControlLoad == false)
            {
                HtmlElement HeaderHE = GS_Web.Document.GetElementById("header_top");
                if (HeaderHE == null)
                {
                    MessageBox.Show("����ѡ�񵽵۹���ҳ");
                    return;
                }
                OGControl.Info.CN();//��ʼ����Ϣ
                HtmlElement SelectHE = HeaderHE.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[1].Children[0].Children[0];
               
                Regex Rx = new Regex(@"(<option)[\s\S]+?(</option>)");
                MatchCollection MatchC = Rx.Matches(SelectHE.InnerHtml.ToLower());

                //���session
                Regex SesRx = new Regex(@"(?<=session=)[\S]+?(?=&)");
                Match SesMc = SesRx.Match(MatchC[0].Value.ToLower());
                _Session = SesMc.Value;

                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

                for (int i = MatchC.Count - 1; i >= 0; i--)
                {
                    string OptionStr = MatchC[i].Value.ToLower();
                    Match PcMc = Regex.Match(OptionStr, @"(?<=cp=)[\S]+?(?=&)"); //�������ID
                    Match PlTextMc = Regex.Match(OptionStr, @"(?<=(<option[\s\S]+?>))[\s\S]+?(?=</option>)");//�����ַ
                    string Location = Regex.Match(PlTextMc.Value, "(?<=\\[)([0-9]?):([0-9])*:(([0-9]?[0-9]?))(?=\\])").Value;//��ַ
                    string GalaxyLocation = Regex.Match(PlTextMc.Value, "[0-9](?=:[0-9]*:)").Value;
                    string SystemLocation = Regex.Match(PlTextMc.Value, "(?<=:)[0-9]+?(?=:)").Value;
                    string PositionLocation = Regex.Match(PlTextMc.Value, "(?<=:[0-9]+?:)[0-9]*").Value;
                    //_PcMc = PcMc;
                    //_PlTextMc = PlTextMc;


                    //System.Threading.ThreadStart start = new System.Threading.ThreadStart(OGadd);
                    //System.Threading.Thread Th = new System.Threading.Thread(start);
                    //Th.ApartmentState = System.Threading.ApartmentState.STA;//���ؼ��� 
                    //Th.SetApartmentState(ApartmentState.STA);
                    //Th.Start();

                    //���뽢�Ӵ�������ۺϹ����ࡣ
                    OGControl.OGControl OGCol = new CR_Galaxy.OGControl.OGControl(_FlottenCmd, _OGControlManage, this);
                    OGCol.Session = _Session;//���ID
                    OGCol.PlanetID = "&cp=" + PcMc.Value;//����ID
                    OGCol.Planet = PlTextMc.Value;
                    OGCol.Location = Location;
                    OGCol.GalaxyLocation = GalaxyLocation;
                    OGCol.SystemLocation = SystemLocation;
                    OGCol.PositionLocation = PositionLocation;
                    //OGCol._FlottenCmd = _FlottenCmd;//д�뽢�Ӵ�����
                    OGCol.WebsiteEx = _SerInfo.WebsiteEx;
                    PlanetControl.Controls.Add(OGCol);
                    OGCol.Dock = DockStyle.Left;
                    OGCol.Enabled = false;
                    OGCol.Visible = true;
                    Application.DoEvents();
                    OGCol.StartLoad();

                    _OGControlManage._OGControl.Add(Location, OGCol);

                    SetFS(OGCol);
                    //_OGControlManage._OGControl.Add(

                   // MessageBox.Show(OGCol._FSGalaxy + OGCol._FSSystem + OGCol._FSPosition + OGCol._FSSheep + OGCol._FSOrder);
                }

                _OGControlManage._Session = SesMc.Value;
                ogMilitary1.WebsiteEx = _SerInfo.WebsiteEx;
                ogMilitary1._FlottenCmd = _FlottenCmd;
                ogMilitary1._OGCOntrolManage = _OGControlManage;
                ogMilitary1.FleetFlyListStart();
                ControlLoad = true;
            }
        }

        private void SetFS(OGControl.OGControl OG)
        {
            string[] FSstr = File.ReadAllLines(Application.StartupPath + "\\Data\\FS_Info.txt");
            for (int i = 0; i < FSstr.Length; i++)
            {
                if (FSstr[i].IndexOf(OG.Location) >= 0)
                {
                    string[] Sq = Regex.Split(FSstr[i], ",");
                    OG._FSGalaxy = Sq[1];
                    OG._FSSystem  = Sq[2];
                    OG._FSPosition  = Sq[3];
                    OG._FSSheep  = Sq[4];
                    OG._FSOrder  = Sq[5];
                }
            }
        }

        //private void OGadd()
        //{
        //    //���뽢�Ӵ�������ۺϹ����ࡣ
        //    OGControl.OGControl OGCol = new CR_Galaxy.OGControl.OGControl(_FlottenCmd, _OGControlManage, this);
        //    OGCol.Session = _Session;//���ID
        //    OGCol.PlanetID = "&cp=" + _PcMc.Value;//����ID
        //    OGCol.Planet = _PlTextMc.Value;
        //    //OGCol._FlottenCmd = _FlottenCmd;//д�뽢�Ӵ�����
        //    OGCol.WebsiteEx = _SerInfo.WebsiteEx;
        //    PlanetControl.Controls.Add(OGCol);


        //    OGCol.Dock = DockStyle.Left;
        //    OGCol.Enabled = false;
        //    OGCol.Visible = true;
        //    Application.DoEvents();
        //    OGCol.StartLoad();


        //}

        private void tabPage16_Enter(object sender, EventArgs e)
        {
            if (webBrowser1.Url == null)
                webBrowser1.Navigate(@"http://www.crsoft.net.cn");
        }

        private void GridMain_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            {
                try
                {
                    if (this.GridMain["Username", e.RowIndex].Value.ToString() != "")
                    {
                        this.TxtSpy.Text = "";
                        string spGalaxySystem = this.GridMain["GalaxySystem", e.RowIndex].Value.ToString();
                        this.TxtSpy.Text = this._IOData.GetDataSpy(spGalaxySystem);
                        if (this.TxtSpy.Text == "")
                        {
                            this.TxtSpy.Text = spGalaxySystem + " �ļ������\r\n";
                        }
                        this.M_Galaxy.Text = spGalaxySystem.Substring(0, 1);
                        spGalaxySystem = spGalaxySystem.Substring(2);
                        this.M_System.Text = spGalaxySystem.Substring(0, spGalaxySystem.IndexOf(":"));
                        this.GridPlanet.DataSource = this._IOData.FindUserOtherPlanet(this.GridMain["Username", e.RowIndex].Value.ToString());
                        this.GroupPlanet.Text = "�û�: " + this.GridMain["Username", e.RowIndex].Value.ToString() + "    ����: " + this.GridMain["Union", e.RowIndex].Value.ToString();
                        if (this.TabInfo.SelectedIndex == 1)
                        {
                            this.GridUserEes.DataSource = this._IOData.HistoryUserRankings("UserEes", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUserFlt.DataSource = this._IOData.HistoryUserRankings("UserFlt", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUserPts.DataSource = this._IOData.HistoryUserRankings("UserPts", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUnionEes.DataSource = null;
                            this.GridUnionFlt.DataSource = null;
                            this.GridUnionPts.DataSource = null;
                        }
                        else if (this.TabInfo.SelectedIndex == 2)
                        {
                            this.GridUnionEes.DataSource = this._IOData.HistoryUnionRankings("UnionEes", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUnionFlt.DataSource = this._IOData.HistoryUnionRankings("UnionFlt", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUnionPts.DataSource = this._IOData.HistoryUnionRankings("UnionPts", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUserEes.DataSource = null;
                            this.GridUserFlt.DataSource = null;
                            this.GridUserPts.DataSource = null;
                        }
                        else
                        {
                            this.TabInfoClear();
                        }
                    }
                    else
                    {
                        this.TxtSpy.Text = "";
                        this.GroupPlanet.Text = "";
                        this.GroupNeighbors.Text = "";
                        this.GridPlanet.DataSource = null;
                        this.GridNeighbors.DataSource = null;
                        this.TabInfoClear();
                    }
                }
                catch (Exception exception)
                {
                    this.Log.AddError("<GridMain_RowEnter_1>" + exception.Message);
                    this.GroupPlanet.Text = "";
                    this.GroupNeighbors.Text = "";
                    this.GridPlanet.DataSource = null;
                    this.GridNeighbors.DataSource = null;
                }
            }

            //try
            //{
            //    if (GridMain["Username", e.RowIndex].Value.ToString().Trim().Length == 0) return;
            //    GridPlanet.DataSource = _IOData.FindUserOtherPlanet(GridMain["Username", e.RowIndex].Value.ToString());
            //    GroupPlanet.Text = "��������  ���:" + GridMain["Username", e.RowIndex].Value.ToString();
            //}
            //catch (Exception ee)
            //{
            //    Log.AddError("<GridMain_RowEnter>" + ee.Message);
            //    GridNeighbors.DataSource = null;
            //}
        }

        private void GridMain_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.TxtLog.Text = this.TxtLog.Text + this.GridMain["GalaxySystem", e.RowIndex].Value.ToString() + "\r\n";

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                msgList.Text = File.ReadAllText(openFileDialog2.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userList.Items.Clear();
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string[] FileText = File.ReadAllLines(openFileDialog2.FileName);
                for (int i = 0; i < FileText.Length; i++)
                {
                    string[] UT= Regex.Split(FileText[i],",");
                    if (UT.Length != 2) continue;
                    ListViewItem LVI = userList.Items.Add(UT[0]);
                    LVI.SubItems.Add(UT[1]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(openFileDialog2.FileName, msgList.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamWriter st = File.AppendText(openFileDialog2.FileName);
                for (int i = 0; i <userList.Items.Count ; i++)
                {
                    st.WriteLine(userList.Items[i].Text + "," + userList.Items[i].SubItems[0].Text);
                }
            }
        }




        private void button5_Click(object sender, EventArgs e)
        {
//Ⱥ�����ܲ�����
            //if (button5.Text == "��ʼ")
            //{
            //    MsgCancel = false;
            //    Random ro = new Random(unchecked((int)DateTime.Now.Ticks));

            //    MsgWB.Navigate(string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "�ޱ���", msgList.Lines[ro.Next(msgList.Lines.Length)] }));

            //    button5.Text = "ֹͣ";
            //}
            //else if (button5.Text == "ֹͣ")
            //{
            //    MsgCancel = true;
            //    button5.Text = "��ʼ";
            //}
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(WebUrl.Text);

        }
        
    }
}