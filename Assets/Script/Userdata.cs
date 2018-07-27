using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Userdata
    {
        public float audioValue = 0.5f;//音量
        public bool haveGameSave = false;//是否有存档
        public bool haveClearGame = false;//是否已经通关

        public int residueLife = 1024;//剩余生命

        public int alreadyClearLevelNumber = 0;//已经通过的关卡数量
        public int alreadyClearMiddleLevelNumber = 0;//在关卡中已经通过的子关卡数量
    }
}