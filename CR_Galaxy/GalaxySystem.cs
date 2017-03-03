using System;
using System.Collections.Generic;
using System.Text;
using CR_Soft.Windows.Web;

namespace CR_Galaxy
{
    public class GalaxySystem : CR_Soft.Windows.Web.ClassWB
    {
        private int _Galaxy;
        private int _System;
        /// <summary>
        /// ��ʼ����
        /// </summary>
        private int _BeginGalaxy;
        /// <summary>
        /// ��ʼ̫��ϵ
        /// </summary>
        private int _BeginSystem;
        /// <summary>
        /// ��������
        /// </summary>
        private int _EndGalaxy;
        /// <summary>
        /// ����̫��ϵ
        /// </summary>
        private int _EndSystem;

        private string GalaxyURL = "{0}index.php?page=galaxy&no_header=1&session={1}&galaxy={2}&system={3}";

        /// <summary>
        /// ����
        /// </summary>
        public GalaxySystem(System.Windows.Forms.WebBrowser spWB)
            : base(spWB)
        {

        }

        /// <summary>
        /// ̫��ϵ
        /// </summary>
        public int System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Galaxy
        {
            get
            {
                return _Galaxy;
            }
            set
            {
                _Galaxy = value;
            }
        }

        public int BeginGalaxy
        {
            get
            {
                return _BeginGalaxy;
            }
            set
            {
                _BeginGalaxy = value;
            }
        }

        public int BeginSystem
        {
            get
            {
                return _BeginSystem;
            }
            set
            {
                _BeginSystem = value;
            }
        }

        public int EndGalaxy
        {
            get
            {
                return _EndGalaxy;
            }
            set
            {
                _EndGalaxy = value;
            }
        }

        public int EndSystem
        {
            get
            {
                return _EndSystem;
            }
            set
            {
                _EndSystem = value;
            }
        }

        /// <summary>
        /// ̫��ϵ��һ
        /// </summary>
        public void SystemInc()
        {
            _System =Convert.ToInt32 ( this.GetSystme());//����̫��ϵ���
            if (_System >= 499)
            {
                _Galaxy++;
                base.ModValueName("galaxy", _Galaxy.ToString());
                _System = 1;
                Goto();
            }
            else
            {
                base.ClickName("systemRight");
                _System++;
            }
        }

        /// <summary>
        /// ���Ӽ�һ
        /// </summary>
        public void GalaxyInc()
        {
            base.ClickName("galaxyRight");
        }

        /// <summary>
        /// ��ת��
        /// </summary>
        /// <param name="spGalaxy">����</param>
        /// <param name="spSystem">̫��ϵ</param>
        public void Goto(int spGalaxy, int spSystem)
        {
            _Galaxy = spGalaxy;
            _System = spSystem;
            Goto();
        }

        /// <summary>
        /// ֱ��ת��
        /// </summary>
        public void Goto()
        {
            base.ModValueName("galaxy", _Galaxy.ToString());
            base.ModValueName("system",Convert.ToString(_System-1));
            //base.ClickValue("��ʾ");//��֪Ϊʲô����ʾ��Ч
            base.ClickName("systemRight"); //�������������
        }

        public void Goto(string Server, string spSession, int spGalaxy, int spSystem)
        {
            _Galaxy = spGalaxy;
            _System = spSystem;
            base.Navigate(string.Format(GalaxyURL, new string[] { Server, spSession, spGalaxy.ToString(), spSystem.ToString() }));
        }

        public void Goto(string Server, string spSession)
        {
            base.Navigate(string.Format(GalaxyURL, new string[] { Server, spSession, _Galaxy.ToString(), _System.ToString () }));
        }

        /// <summary>
        /// ��ҳ���ȡ̫��ϵ��ַ
        /// </summary>
        /// <returns></returns>
        public string GetSystme()
        {
           return base.ReadValueName("system");
        }

        /// <summary>
        /// ��ҳ���ȡ����ϵ��ַ
        /// </summary>
        /// <returns></returns>
        public string GetGalaxy()
        {
            return base.ReadValueName("galaxy");
        }

        /// <summary>
        /// ��ϵ��ַ�Ƿ����
        /// </summary>
        /// <returns>
        /// 0������
        /// 1�����ӵ�ַ���
        /// 2��̫��ϵ��ַ���
        /// 3�������
        /// 4������������Χ
        /// </returns>
        public int Overflow(int spGalaxy, int spSystem)
        {
            if (spGalaxy > _EndGalaxy) return 4;
            if (spGalaxy >= _EndGalaxy)
            {
                if (spSystem > _EndSystem)
                {
                    return 4;
                }

            }
            return 0;
        }
        /// <summary>
        /// ��ַ�Ƿ����
        /// </summary>
        /// <returns></returns>
        public int Overflow()
        {
            if (_Galaxy > _EndGalaxy) return 4;

            if (_Galaxy >= _EndGalaxy)
            {
                if (_System > _EndSystem)
                {
                    return 4;
                }

            }
            return 0;
        }

        /// <summary>
        /// �����ϵ����
        /// </summary>
        /// <returns></returns>
        public int GetSystemCount()
        {
            if (_BeginGalaxy == _EndGalaxy)
            {
                return _EndSystem - _BeginSystem;

            }
            else
            {
                return (_EndGalaxy - _BeginGalaxy - 1) * 499 + (499 - _BeginSystem) + _EndSystem + 1 ;
            }
        }
    }
}