using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    public struct ExPlanet
    {
        /// <summary>
        /// ����
        /// </summary>
        public int Galaxy;
        /// <summary>
        /// ̫��ϵ
        /// </summary>
        public int System;
        /// <summary>
        /// λ��
        /// </summary>
        public int Location;
        /// <summary>
        /// ������
        /// </summary>
        public string PlanetName;
        /// <summary>
        /// ����
        /// </summary>
        public bool Moon;
        /// <summary>
        /// ���
        /// </summary>
        public int MoonSize;
        /// <summary>
        /// ����
        /// </summary>
        public int Metal;
        /// <summary>
        /// ����
        /// </summary>
        public int Crystal;
        /// <summary>
        /// �û���
        /// </summary>
        public string Username;
        /// <summary>
        /// ����
        /// </summary>
        public string Union;
        /// <summary>
        /// ����
        /// </summary>
        public bool Vacation;
        /// <summary>
        /// 7�첻����
        /// </summary>
        public bool Inactive;
        /// <summary>
        /// 30�첻����
        /// </summary>
        public bool LongInactive;
        /// <summary>
        /// ����
        /// </summary>
        public bool Banned;
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Date;

        public string Memo;

        public string Spy;

        /// <summary>
        /// ��������
        /// </summary>
        public int UnionRankings;
        /// <summary>
        /// ���˳�Ա
        /// </summary>
        public int Members;
        /// <summary>
        /// ����
        /// </summary>
        public int Rankings;

        public ExPlanet(bool spbool)
        {
            this.Galaxy = 0;
            System = 0;
            Location = 0;
            MoonSize = 0;
            Metal = 0;
            Crystal = 0;

            UnionRankings = 0;
            Members = 0;
            this.Rankings = 0;

            PlanetName = "";
            Username = "";
            Union = "";


            Banned = false;
            LongInactive = false;
            Inactive = false;
            Vacation = false;
            Moon = false;

            Date = new DateTime() ;
            Memo = "";
            Spy = "";
        }
    }
}
