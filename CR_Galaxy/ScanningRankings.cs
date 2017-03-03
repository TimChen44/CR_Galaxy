using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CR_Soft.ClassLibrary.Log;
using CR_Soft.ClassLibrary;


namespace CR_Galaxy
{
    class ScanningRankings
    {
        private Navigate _Navigate;
        private ServerInfo _ServerInfo;
        private WebBrowser _WB;
        private IOData _IOData;
        private Log _Log;


        private string _Stat = "{0}index.php?page=statistics&session={1}&who={2}&type={3}&start={4}&sort_per_member={5}";
        private string[] _PlayerStart = new string[] {"1","101","201","301","401","501","601","701","801","901","1001","1101","1201","1301","1401","1501","1601","1701","1801","1901","2001",
            "2101","2201","2301","2401","2501","2601","2701","2801","2901","3001","3101","3201","3301","3401","3501","3601","3701","3801","3901","4001","4101","4201","4301","4401",
            "4501","4601","4701","4801","4901","5001","5101","5201","5301","5401","5501","5601","5701","5801","5901","6001","6101","6201","6301","6401","6501","6601","6701","6801",
            "6901","7001" };
        private string[] _Type = new string[] { "ressources", "fleet", "research" };
        private string[] _Who = new string[] { "player", "ally" };

        private int _PlayerStartI;
        private int _TypeI;
        private int _WhoI;

        private string _OldName;

        private bool[] ScanningObject = new bool[] { true, true, true };//��¼ɨ����Щ����

        private int Count;

        public ScanningRankings(Navigate spNavigate, ServerInfo spServerInfo, WebBrowser spWB, IOData spIOData, CR_Soft.ClassLibrary.Log.Log spLog)
        {
            _Navigate = spNavigate;
            _ServerInfo = spServerInfo;
            _WB = spWB;
            _IOData = spIOData;
            _Log = spLog;
        }


        private string UrlFormat(string spWho, string spType, string spStart)
        {
            return string.Format(_Stat, new string[] { _ServerInfo.WebsiteEx, _Navigate._Session, spWho, spType, spStart,"0" });
        }


        /// <summary>
        /// ���������һ��ɨ���Ǹ����
        /// </summary>
        /// <returns></returns>
        private int BegingType()
        {
            for (int i = 0; i < 3; i++)
            {
                if (ScanningObject[i] == true)
                    return i;
            }
            return 0;//���ѭ������ʱΪ3��ô��ȫ��ɨ��
        }

        /// <summary>
        /// ��ʼ����ɨ��
        /// </summary>
        public void BeginPlayer()
        {
            if (_Navigate._Session == null)
            {
                _Navigate.SaveSession(_WB.Document);
            }
            _PlayerStartI = 0;
            _TypeI = BegingType();
            _WhoI = 0;
            Count = 0;

            _WB.Navigate(UrlFormat(_Who[0], _Type[_TypeI], _PlayerStart[0]));
        }

        /// <summary>
        /// ����ɨ��
        /// </summary>
        public bool MainPlayer()
        {

            Application.DoEvents();
            if (_WB.Document.GetElementById("content") == null)
            {
                _WB.Navigate(_Navigate.EndUrl);
                return true;
            }
           

            Rankings[] lRankings = new Rankings[100];

            bool TmpInfo = GetPlayerInfo(lRankings);
            if (TmpInfo == false)
            {
                _WB.Navigate(_Navigate.EndUrl);
                _Log.AddInformation ("<MainPlayer>" + "\"GetPlayerInfo\"��������ɨ��" + GetPlayerStart());
                return true;
            }

            //���ݱ������
            if (_TypeI == 0)//����
            {
                _IOData.OutRankings(lRankings, "UserPts");
                _IOData.UpDataRankings(lRankings);
            }
            else if (_TypeI == 1)//����
            {
                _IOData.OutRankings(lRankings, "UserFlt");
            }
            else if (_TypeI == 2)//�о�
            {
                _IOData.OutRankings(lRankings, "UserEes");
            }

            Count++;

            if (lRankings[99].Name == _OldName )
            {
                _OldName = "";
                return PlayerNextPage(true);
            }
            else
            {
                _OldName = lRankings[99].Name;
                return PlayerNextPage(false );
                
            }

           

        }

        public void BeginUnion()
        {
            if (_Navigate._Session == null)
            {
                _Navigate.SaveSession(_WB.Document);
            }
            _PlayerStartI = 0;
            _TypeI = BegingType();
            _WhoI = 1;
            Count = 0;

            _WB.Navigate(UrlFormat(_Who[1], _Type[_TypeI], _PlayerStart[0]));
        }

        public bool MainUnion()
        {

            Application.DoEvents();
            if (_WB.Document.GetElementById("content") == null)
            {
                _WB.Navigate(_Navigate.EndUrl);
                return true;
            }


            Rankings[] lRankings = new Rankings[100];
            int[] Members=new int[100];

            bool TmpInfo = GetUnionInfo(lRankings, Members);
            if (TmpInfo == false)
            {
                _WB.Navigate(_Navigate.EndUrl);
                _Log.AddInformation("<MainPlayer>" + "\"GetUnionInfo\"��������ɨ��" + GetPlayerStart());
                return true;
            }
            else
            {
                Count++;
            }

            //���ݱ������
            if (_TypeI == 0)//����
            {
                _IOData.OutUnionRankings(lRankings, Members, "UnionPts");
                _IOData.UpDataUnionRankings(lRankings, Members);
            }
            else if (_TypeI == 1)//����
            {
                _IOData.OutUnionRankings(lRankings, "UnionFlt");
            }
            else if (_TypeI == 2)//�о�
            {
                _IOData.OutUnionRankings(lRankings, "UnionEes");
            }

            

            if (lRankings[99].Name == null)
            {
                return PlayerNextPage(true);
            }
            else
            {
                return PlayerNextPage(false);
            }
        }

        public bool  GetPlayerInfo(Rankings[] spRankings)
        {
            try
            {
            HtmlElement ht = _WB.Document.GetElementById("content");
            HtmlElement td = ht.Children[0].Children[5].Children[0];//�õ���

               string TimeStr= _WB.Document.Forms[0].Children[1].Children[0].Children[0].Children[0].InnerText ;
               TimeStr = TimeStr.Substring(TimeStr.IndexOf(": ") + 2, TimeStr.IndexOf(")") - TimeStr.IndexOf(": ") - 2);
               TimeStr = TimeStr.Replace(",", "");
               spRankings[0].Date = TimeStr;//Ϊ�˼��ٲ�����������ֻ�õ�һ������������ʱ��

                for (int i = 0; i < td.Children.Count -1; i++)
                {
                    spRankings[i].EesR = Convert.ToInt32(td.Children[i + 1].Children[1].InnerText.Substring(0, td.Children[i + 1].Children[1].InnerText.IndexOf(" ")).Trim());//����

                    if (td.Children[i + 1].Children[1].Children[0].Children.Count == 1)
                    {
                        string tmp = td.Children[i + 1].Children[1].InnerHtml;
                        tmp = tmp.Substring(tmp.IndexOf(">") + 1, tmp.IndexOf("</font>") - tmp.IndexOf(">") - 1);
                        if (tmp == "*")
                            spRankings[i].EesC = 0;
                        else
                            spRankings[i].EesC = Convert.ToInt32(tmp);
                       
                    }
                    else
                    {
                        spRankings[i].EesC = 0;
                    }
                    spRankings[i].Name = td.Children[i + 1].Children[3].InnerText.Trim();//���
                    spRankings[i].EesS = Convert.ToInt32(td.Children[i + 1].Children[9].InnerText.Replace(".", "").Trim());//����

                }

                return true;
            }
            catch (Exception ee)
            {
                _Log.AddError("<GetPlayerInfo>" + ee.Message);
                return false;
            }
        }

        public bool GetUnionInfo(Rankings[] spRankings, int[] Members)
        {
            try
            {
                HtmlElement ht = _WB.Document.GetElementById("content");
                HtmlElement td = ht.Children[0].Children[5].Children[0];//�õ���

                string TimeStr = _WB.Document.Forms[0].Children[1].Children[0].Children[0].Children[0].InnerText;
                TimeStr = TimeStr.Substring(TimeStr.IndexOf(": ") + 2, TimeStr.IndexOf(")") - TimeStr.IndexOf(": ") - 2);
                TimeStr = TimeStr.Replace(",", "");
                spRankings[0].Date = TimeStr;//Ϊ�˼��ٲ�����������ֻ�õ�һ������������ʱ��

                for (int i = 0; i < td.Children.Count - 1; i++)
                {
                    spRankings[i].EesR = Convert.ToInt32(td.Children[i + 1].Children[1].InnerText.Substring(0, td.Children[i + 1].Children[1].InnerText.IndexOf(" ")).Trim());//����

                    if (td.Children[i + 1].Children[1].Children[0].Children.Count == 1)
                    {
                        string tmp = td.Children[i + 1].Children[1].InnerHtml;
                        tmp = tmp.Substring(tmp.IndexOf(">") + 1, tmp.IndexOf("</font>") - tmp.IndexOf(">") - 1);
                        if (tmp == "*")
                            spRankings[i].EesC = 0;
                        else
                            spRankings[i].EesC = Convert.ToInt32(tmp);
                        
                    }
                    else
                    {
                        spRankings[i].EesC = 0;
                    }
                    spRankings[i].Name = td.Children[i + 1].Children[3].InnerText.Trim();//������
                    spRankings[i].EesS = Convert.ToInt32(td.Children[i + 1].Children[9].InnerText.Replace(".", "").Trim());//����


                    Members[i] = Convert.ToInt32(td.Children[i + 1].Children[7].InnerText.Replace(".", "").Trim());//���˳�Ա��
                }

                return true;
            }
            catch (Exception ee)
            {
                _Log.AddError("<GetUnionInfo>" + ee.Message);
                return false;
            }

        }


/// <summary>
        /// ת����һҳ���������false��ôɨ�����
/// </summary>
/// <param name="Rest">TrueΪֱ��ɨ����һ��</param>
/// <returns></returns>
        private bool PlayerNextPage(bool Rest)
        {
            try
            {


                if (Rest == true)
                {
                    if (_TypeI == 2)
                    {
                        return false;
                    }
                    else
                    {
                        _TypeI++;
                        while (_TypeI < 3)
                        {
                            if (ScanningObject[_TypeI] == true)
                                break;
                            _TypeI++;
                        } 
                        if (_TypeI == 3) return false;//���ѭ������ʱΪ3��ô˵�������Ѿ�û��Ҫɨ���������
                        _PlayerStartI = 0;
                    }
                }
                else
                {
                    _PlayerStartI++;
                }
                _WB.Navigate(UrlFormat(_Who[_WhoI], _Type[_TypeI], _PlayerStart[_PlayerStartI]));
                return true;

            }
            catch (Exception ee)
            {
                _Log.AddError("<PlayerNextPage>" + ee.Message);
                return false;
            }
        }

        /// <summary>
        /// ��õ�ǰɨ���λ��
        /// </summary>
        /// <returns></returns>
        public string GetPlayerStart()
        {
            try
            {


                return _PlayerStart[_PlayerStartI];
            }
            catch (Exception ee)
            {
                _Log.AddError("<GetPlayerStart>" + ee.Message);
                return _PlayerStartI.ToString();
            }
        }

        public int GetSacCount()
        {
            return Count;
        }

        /// <summary>
        /// ����ɨ�����
        /// </summary>
        /// <param name="sp1"></param>
        /// <param name="sp2"></param>
        /// <param name="sp3"></param>
        public void SetScanningObject(bool sp1, bool sp2, bool sp3)
        {
            ScanningObject[0] = sp1;
            ScanningObject[1] = sp2;
            ScanningObject[2] = sp3;
        }




    }
}
