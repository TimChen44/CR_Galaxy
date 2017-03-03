using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    /// <summary>
    /// ����-����
    /// </summary>
    /// <remarks></remarks>
    public struct Planet
    {
        /// <summary>
        /// λ��
        /// </summary>
        public int Location;
        /// <summary>
        /// ����
        /// </summary>
        public bool Moon ;
        /// <summary>
        /// ������
        /// </summary>
        public string PlanetName;
        /// <summary>
        /// �û���
        /// </summary>
        public string Username;
        /// <summary>
        /// ����
        /// </summary>
        public int Rankings;
        /// <summary>
        /// ����
        /// </summary>
        public string Union;
        /// <summary>
        /// ��������
        /// </summary>
        public int UnionRankings;
        /// <summary>
        /// ���˳�Ա
        /// </summary>
        public int Members;
        ///// <summary>
        ///// �͵ȼ�
        ///// </summary>
        //public bool Moon;
        /// <summary>
        /// ����
        /// </summary>
        public bool Vacation ;
        /// <summary>
        /// 7�첻����
        /// </summary>
        public bool Inactive ;
        /// <summary>
        /// 30�첻����
        /// </summary>
        public bool LongInactive;
        /// <summary>
        /// ����
        /// </summary>
        public bool Banned ;
        /// <summary>
        /// ����
        /// </summary>
        public int Galaxy;
        /// <summary>
        /// ̫��ϵ
        /// </summary>
        public int System;
        /// <summary>
        /// ����
        /// </summary>
        public string Date;

        public string Memo;

        public string Spy;

        public Planet(bool spbool)
        {
            Location = 0;
            Rankings = 0;
            UnionRankings = 0;
            Members = 0;
            this.Galaxy = 0;
            System = 0;

            PlanetName = "";
            Username = "";
            Union = "";

            Banned = false;
            LongInactive = false;
            Inactive = false;
            Vacation = false;
            Moon = false;
            Moon = false;

            Date = "";
            Memo = "";
            Spy = "";
        }



    }

}
