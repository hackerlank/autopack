﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace autopack
{
    [Serializable]
    class InitBundle : SerailizeXml
    {
        void runDelete(string nDirectory)
        {
            DirectoryInfo directoryInfo_ = new DirectoryInfo(nDirectory);
            foreach (FileInfo fileInfo_ in directoryInfo_.GetFiles())
            {
                File.Delete(fileInfo_.FullName);
            }
            foreach (DirectoryInfo suDirectoryInfo_ in directoryInfo_.GetDirectories())
            {
                runDelete(suDirectoryInfo_.FullName);
            }
            Directory.Delete(nDirectory);
        }

        void runInit(Bundle nBundle)
        {
            string apk_ = nBundle.mDirectorys["apk"];
            if (Directory.Exists(apk_))
            {
                this.runDelete(apk_);
            }
            Directory.CreateDirectory(apk_);

            string update_ = nBundle.mDirectorys["update"];
            if (Directory.Exists(update_))
            {
                this.runDelete(update_);
            }
            Directory.CreateDirectory(update_);
        }

        public void runCommand(string nCommand, Bundle nBundle)
        {
            string versionNoXml_ = nBundle.mDirectorys["versionNo"];
            VersionNo versionNo_ = Deserialize<VersionNo>(versionNoXml_);

            foreach (DeleteOnce i in mDeleteOnces)
            {
                i.runDelete(nBundle);
            }
            if ("apk" == nCommand)
            {
                versionNo_.mApkNo += 1;
                versionNo_.mUpdateNo = 1;
            }
            else if ("update" == nCommand)
            {
                versionNo_.mUpdateNo += 1;
            }
            Serialize<VersionNo>(versionNoXml_, versionNo_);
        }

        public List<DeleteOnce> mDeleteOnces { get; set; }
    }
}
