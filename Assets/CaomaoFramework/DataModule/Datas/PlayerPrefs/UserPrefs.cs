using System;
using System.Xml;
using System.IO;
using Path = System.IO.Path;
namespace CaomaoFramework
{
    public class UserPrefsBase
    {
        private const string UserConfigFile = "config/user/useroptionscfg.xml";
        private bool m_bHasSet = false;
        private int m_nDisplayMode = 1;
        private bool m_bMuteSound = false;
        private float m_fBGSoundValue = 0.5f;
        private float m_fUISoundValue = 0.75f;
        private float m_fSoundValue = 0.5f;
        private float m_fVoiceValue = 1f;
        private GraphicsQuality m_eGraphicsQuality = GraphicsQuality.Medium;
        private static UserPrefsBase s_instance = null;
        private ICLog m_log = CLog.GetLog<UserPrefsBase>();
        public static UserPrefsBase Singleton
        {
            get
            {
                if (UserPrefsBase.s_instance == null)
                {
                    UserPrefsBase.s_instance = new UserPrefsBase();
                    UserPrefsBase.s_instance.Init();
                }
                return UserPrefsBase.s_instance;
            }
        }
        public bool HasSet
        {
            get
            {
                return this.m_bHasSet;
            }
        }
        public int DisplayMode
        {
            get
            {
                return this.m_nDisplayMode;
            }
            set
            {
                this.m_nDisplayMode = value;
            }
        }
        public bool IsMute
        {
            get
            {
                return this.m_bMuteSound;
            }
            set
            {
                this.m_bMuteSound = value;
            }
        }
        public float BGSoundValue
        {
            get
            {
                return this.m_fBGSoundValue;
            }
            set
            {
                this.m_fBGSoundValue = value;
            }
        }
        public float UISoundValue
        {
            get
            {
                return this.m_fUISoundValue;
            }
            set
            {
                this.m_fUISoundValue = value;
            }
        }
        public float SoundValue
        {
            get
            {
                return this.m_fSoundValue;
            }
            set
            {
                this.m_fSoundValue = value;
            }
        }
        public float VoiceValue
        {
            get
            {
                return this.m_fVoiceValue;
            }
            set
            {
                this.m_fVoiceValue = value;
            }
        }
        public GraphicsQuality Quality
        {
            get
            {
                return this.m_eGraphicsQuality;
            }
            set
            {
                this.m_eGraphicsQuality = value;
            }
        }
        public UserPrefsBase()
        {
               
        }
        public virtual void Init()
        {
            try
            {
                this.LoadUserConfig();
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        public virtual void SaveUserConfig()
        {
            //string fullPath = ResourceModule.GetFullPath("config/user/useroptionscfg.xml", false);
            string fullPath = "";
            string directoryName = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(newChild);
            XmlElement xmlElement = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(xmlElement);
            //XmlElement xmlElement6 = xmlDocument.CreateElement("element");
            //xmlElement6.SetAttribute("id", "resolution");
            //xmlElement6.SetAttribute("value", string.Format("{0},{1}", this.m_IGraphicsResolution.Width, this.m_IGraphicsResolution.Height));
            //xmlElement.AppendChild(xmlElement6);
            XmlElement xmlElement7 = xmlDocument.CreateElement("element");
            xmlElement7.SetAttribute("id", "mutesound");
            xmlElement7.SetAttribute("value", this.m_bMuteSound ? 1.ToString() : 0.ToString());
            xmlElement.AppendChild(xmlElement7);
            XmlElement xmlElement8 = xmlDocument.CreateElement("element");
            xmlElement8.SetAttribute("id", "bgsoundvalue");
            xmlElement8.SetAttribute("value", this.m_fBGSoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement8);
            XmlElement xmlElement9 = xmlDocument.CreateElement("element");
            xmlElement9.SetAttribute("id", "uisoundvalue");
            xmlElement9.SetAttribute("value", this.m_fUISoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement9);
            XmlElement xmlElement10 = xmlDocument.CreateElement("element");
            xmlElement10.SetAttribute("id", "soundvalue");
            xmlElement10.SetAttribute("value", this.m_fSoundValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement10);
            XmlElement xmlElement11 = xmlDocument.CreateElement("element");
            xmlElement11.SetAttribute("id", "voicevalue");
            xmlElement11.SetAttribute("value", this.m_fVoiceValue.ToString("f2"));
            xmlElement.AppendChild(xmlElement11);
            XmlElement xmlElement12 = xmlDocument.CreateElement("element");
            xmlElement12.SetAttribute("id", "qualitysetting");
            xmlElement12.SetAttribute("value", string.Format("{0}", (int)this.m_eGraphicsQuality));
            xmlElement.AppendChild(xmlElement12);          
            xmlDocument.Save(fullPath);
        }
        public virtual void LoadUserConfig()
        {
            //string fullPath = ResourceModule.GetFullPath("config/user/useroptionscfg.xml", false);
            string fullPath = "";
            if (File.Exists(fullPath))
            {
                using (XmlReader xmlReader = XmlReader.Create(fullPath))
                {
                    if (null != xmlReader)
                    {
                        this.m_bHasSet = true;
                        while (xmlReader.Read())
                        {
                            if (xmlReader.Name == "element" && xmlReader.NodeType == XmlNodeType.Element)
                            {
                                string text = xmlReader.GetAttribute("id").ToLower();
                                string attribute = xmlReader.GetAttribute("value");
                                string text2 = text;
                                switch (text2)
                                {
                                    //case "resolution":
                                    //    {
                                    //        string[] array = attribute.Split(new char[]
                                    //{
                                    //    ','
                                    //});
                                    //        if (array.Length == 2)
                                    //        {
                                    //            int nWidth = Convert.ToInt32(array[0]);
                                    //            int nHeight = Convert.ToInt32(array[1]);
                                    //            this.m_IGraphicsResolution = GraphicsManager.CreateResolution(nWidth, nHeight);
                                    //        }
                                    //        break;
                                    //    }
                                    case "mutesound":
                                        this.m_bMuteSound = (Convert.ToInt32(attribute) != 0);
                                        break;
                                    case "bgsoundvalue":
                                        this.m_fBGSoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "uisoundvalue":
                                        this.m_fUISoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "soundvalue":
                                        this.m_fSoundValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "voicevalue":
                                        this.m_fVoiceValue = (float)Convert.ToDouble(attribute);
                                        break;
                                    case "qualitysetting":
                                        this.m_eGraphicsQuality = (GraphicsQuality)Convert.ToInt32(attribute);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
