using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CR_Galaxy
{
    public class Navigate
    {
        /// <summary>
        /// ������������
        /// </summary>
        public string EndUrl;
        /// <summary>
        /// ��ǰ״̬
        /// 0Ϊ����
        /// 1Ϊæµ
        /// -1Ϊ������
        /// </summary>
        public int State;

        /// <summary>
        /// �洢��ǰWEB����ʲô
        /// 0��ʲô������
        /// 1��ɨ����ͼ
        /// 2��ɨ���������
        /// 3��ɨ�轢������
        /// 10���״ε���
        /// </summary>
        public int Work;

        /// <summary>
        /// ��ǰ״̬
        /// True����ɨ��
        /// Falseɨ�����
        /// </summary>
        public bool WorkState;

        /// <summary>
        /// 
        /// </summary>
        public string _Session;

        public Navigate()
        {

        }

        /// <summary>
        /// ���session
        /// �������ﻹҪ���������Ϣ
        /// </summary>
        /// <param name="spHD"></param>
        /// <returns></returns>
        public bool SaveMainSession(HtmlDocument spHD)
        {
            HtmlElementCollection dom = spHD.GetElementsByTagName("option");
            foreach (HtmlElement HE in dom)
            {
                if (HE.GetAttribute("Value") != null)
                {
                    if (HE.GetAttribute("Value").ToString().ToLower().IndexOf("session") >= 0)
                    {
                        string Tmp = HE.GetAttribute("Value").ToString().ToLower();
                        _Session = Tmp.Substring(Tmp.IndexOf("session=") + 8, 12);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SaveSession(HtmlDocument spHD)
        {
            HtmlElementCollection dom = spHD.GetElementsByTagName("a");
            foreach (HtmlElement HE in dom)
            {
                if (HE.InnerHtml.IndexOf("session=") > 0)
                {
                    _Session = HE.InnerHtml.Substring(HE.InnerHtml.IndexOf("session=") + 8, 12);
                    return true;
                }

            }
            return false;
        }
    }
}
