﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileTransfer.WinTest.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileTransferBase", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    [System.SerializableAttribute()]
    public partial class FileTransferBase : FileTransfer.WinTest.ServiceReference1.BaseItemIdentifiers {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid CloneOfField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid CloneOfVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DecompressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private FileTransfer.WinTest.ServiceReference1.FileTransferType DiscriminatorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FilenameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InfoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ModifiedOnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private FileTransfer.WinTest.ServiceReference1.TransferPolicy PolicyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private FileTransfer.WinTest.ServiceReference1.RepositoryIdentifier RepositoryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private FileTransfer.WinTest.ServiceReference1.TransferStatus StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool isCompletedField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid CloneOf {
            get {
                return this.CloneOfField;
            }
            set {
                if ((this.CloneOfField.Equals(value) != true)) {
                    this.CloneOfField = value;
                    this.RaisePropertyChanged("CloneOf");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid CloneOfVersion {
            get {
                return this.CloneOfVersionField;
            }
            set {
                if ((this.CloneOfVersionField.Equals(value) != true)) {
                    this.CloneOfVersionField = value;
                    this.RaisePropertyChanged("CloneOfVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Decompress {
            get {
                return this.DecompressField;
            }
            set {
                if ((this.DecompressField.Equals(value) != true)) {
                    this.DecompressField = value;
                    this.RaisePropertyChanged("Decompress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public FileTransfer.WinTest.ServiceReference1.FileTransferType Discriminator {
            get {
                return this.DiscriminatorField;
            }
            set {
                if ((this.DiscriminatorField.Equals(value) != true)) {
                    this.DiscriminatorField = value;
                    this.RaisePropertyChanged("Discriminator");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Filename {
            get {
                return this.FilenameField;
            }
            set {
                if ((object.ReferenceEquals(this.FilenameField, value) != true)) {
                    this.FilenameField = value;
                    this.RaisePropertyChanged("Filename");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Info {
            get {
                return this.InfoField;
            }
            set {
                if ((object.ReferenceEquals(this.InfoField, value) != true)) {
                    this.InfoField = value;
                    this.RaisePropertyChanged("Info");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ModifiedOn {
            get {
                return this.ModifiedOnField;
            }
            set {
                if ((this.ModifiedOnField.Equals(value) != true)) {
                    this.ModifiedOnField = value;
                    this.RaisePropertyChanged("ModifiedOn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Path {
            get {
                return this.PathField;
            }
            set {
                if ((object.ReferenceEquals(this.PathField, value) != true)) {
                    this.PathField = value;
                    this.RaisePropertyChanged("Path");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public FileTransfer.WinTest.ServiceReference1.TransferPolicy Policy {
            get {
                return this.PolicyField;
            }
            set {
                if ((this.PolicyField.Equals(value) != true)) {
                    this.PolicyField = value;
                    this.RaisePropertyChanged("Policy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public FileTransfer.WinTest.ServiceReference1.RepositoryIdentifier Repository {
            get {
                return this.RepositoryField;
            }
            set {
                if ((object.ReferenceEquals(this.RepositoryField, value) != true)) {
                    this.RepositoryField = value;
                    this.RaisePropertyChanged("Repository");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public FileTransfer.WinTest.ServiceReference1.TransferStatus Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isCompleted {
            get {
                return this.isCompletedField;
            }
            set {
                if ((this.isCompletedField.Equals(value) != true)) {
                    this.isCompletedField = value;
                    this.RaisePropertyChanged("isCompleted");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DomainBaseObjectOflong", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.DomainModel")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(FileTransfer.WinTest.ServiceReference1.BaseItemIdentifiers))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(FileTransfer.WinTest.ServiceReference1.FileTransferBase))]
    public partial class DomainBaseObjectOflong : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private FileTransfer.WinTest.ServiceReference1.BaseStatusDeleted Deletedk__BackingFieldField;
        
        private long Idk__BackingFieldField;
        
        private byte[] TimeStampk__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Deleted>k__BackingField", IsRequired=true)]
        public FileTransfer.WinTest.ServiceReference1.BaseStatusDeleted Deletedk__BackingField {
            get {
                return this.Deletedk__BackingFieldField;
            }
            set {
                if ((this.Deletedk__BackingFieldField.Equals(value) != true)) {
                    this.Deletedk__BackingFieldField = value;
                    this.RaisePropertyChanged("Deletedk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Id>k__BackingField", IsRequired=true)]
        public long Idk__BackingField {
            get {
                return this.Idk__BackingFieldField;
            }
            set {
                if ((this.Idk__BackingFieldField.Equals(value) != true)) {
                    this.Idk__BackingFieldField = value;
                    this.RaisePropertyChanged("Idk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<TimeStamp>k__BackingField", IsRequired=true)]
        public byte[] TimeStampk__BackingField {
            get {
                return this.TimeStampk__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.TimeStampk__BackingFieldField, value) != true)) {
                    this.TimeStampk__BackingFieldField = value;
                    this.RaisePropertyChanged("TimeStampk__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseItemIdentifiers", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(FileTransfer.WinTest.ServiceReference1.FileTransferBase))]
    public partial class BaseItemIdentifiers : FileTransfer.WinTest.ServiceReference1.DomainBaseObjectOflong {
        
        private long IdItemk__BackingFieldField;
        
        private long IdVersionk__BackingFieldField;
        
        private System.Guid UniqueIdItemk__BackingFieldField;
        
        private System.Guid UniqueIdVersionk__BackingFieldField;
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IdItem>k__BackingField", IsRequired=true)]
        public long IdItemk__BackingField {
            get {
                return this.IdItemk__BackingFieldField;
            }
            set {
                if ((this.IdItemk__BackingFieldField.Equals(value) != true)) {
                    this.IdItemk__BackingFieldField = value;
                    this.RaisePropertyChanged("IdItemk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IdVersion>k__BackingField", IsRequired=true)]
        public long IdVersionk__BackingField {
            get {
                return this.IdVersionk__BackingFieldField;
            }
            set {
                if ((this.IdVersionk__BackingFieldField.Equals(value) != true)) {
                    this.IdVersionk__BackingFieldField = value;
                    this.RaisePropertyChanged("IdVersionk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<UniqueIdItem>k__BackingField", IsRequired=true)]
        public System.Guid UniqueIdItemk__BackingField {
            get {
                return this.UniqueIdItemk__BackingFieldField;
            }
            set {
                if ((this.UniqueIdItemk__BackingFieldField.Equals(value) != true)) {
                    this.UniqueIdItemk__BackingFieldField = value;
                    this.RaisePropertyChanged("UniqueIdItemk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<UniqueIdVersion>k__BackingField", IsRequired=true)]
        public System.Guid UniqueIdVersionk__BackingField {
            get {
                return this.UniqueIdVersionk__BackingFieldField;
            }
            set {
                if ((this.UniqueIdVersionk__BackingFieldField.Equals(value) != true)) {
                    this.UniqueIdVersionk__BackingFieldField = value;
                    this.RaisePropertyChanged("UniqueIdVersionk__BackingField");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseStatusDeleted", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.DomainModel")]
    public enum BaseStatusDeleted : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Manual = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Automatic = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Cascade = 4,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RepositoryIdentifier", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    [System.SerializableAttribute()]
    public partial class RepositoryIdentifier : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int IdCommunityk__BackingFieldField;
        
        private int IdPersonk__BackingFieldField;
        
        private FileTransfer.WinTest.ServiceReference1.RepositoryType Typek__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IdCommunity>k__BackingField", IsRequired=true)]
        public int IdCommunityk__BackingField {
            get {
                return this.IdCommunityk__BackingFieldField;
            }
            set {
                if ((this.IdCommunityk__BackingFieldField.Equals(value) != true)) {
                    this.IdCommunityk__BackingFieldField = value;
                    this.RaisePropertyChanged("IdCommunityk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<IdPerson>k__BackingField", IsRequired=true)]
        public int IdPersonk__BackingField {
            get {
                return this.IdPersonk__BackingFieldField;
            }
            set {
                if ((this.IdPersonk__BackingFieldField.Equals(value) != true)) {
                    this.IdPersonk__BackingFieldField = value;
                    this.RaisePropertyChanged("IdPersonk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Type>k__BackingField", IsRequired=true)]
        public FileTransfer.WinTest.ServiceReference1.RepositoryType Typek__BackingField {
            get {
                return this.Typek__BackingFieldField;
            }
            set {
                if ((this.Typek__BackingFieldField.Equals(value) != true)) {
                    this.Typek__BackingFieldField = value;
                    this.RaisePropertyChanged("Typek__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileTransferType", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    public enum FileTransferType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unmanaged = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Scorm = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Multimedia = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VideoStreaming = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="TransferPolicy", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    public enum TransferPolicy : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        none = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        skipAnalysis = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        deletePreviousFiles = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        deletePreviousAnalysis = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TransferStatus", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    public enum TransferStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Multimedia_NoCandidates = -21,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Multimedia_AnalyzeError = -20,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Scorm_AnalyzeError = -10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FileTypeError = -6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UnableToDeleteAfterUnzip = -5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UnzipFileNotFound = -4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UploadFileNotFound = -3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UnableToUnzip = -2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = -1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyForTransfer = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Copying = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyToUnzip = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unzipping = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unzipped = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyToDelete = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Completed = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deleting = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deleted = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyToAnalyze = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Analyzed = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Suspended = 11,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RepositoryType", Namespace="http://schemas.datacontract.org/2004/07/lm.Comol.Core.FileRepository.Domain")]
    public enum RepositoryType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Community = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Portal = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        User = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IFileMQService")]
    public interface IFileMQService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/FileTransferNotification")]
        void FileTransferNotification(string Platform);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/FileTransferNotification")]
        System.Threading.Tasks.Task FileTransferNotificationAsync(string Platform);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/TransferAllFilesDirect")]
        void TransferAllFilesDirect(string Platform, FileTransfer.WinTest.ServiceReference1.FileTransferBase[] toTransfer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/TransferAllFilesDirect")]
        System.Threading.Tasks.Task TransferAllFilesDirectAsync(string Platform, FileTransfer.WinTest.ServiceReference1.FileTransferBase[] toTransfer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/DirAllFilesDirect")]
        void DirAllFilesDirect(string Platform, string[] Files);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileMQService/DirAllFilesDirect")]
        System.Threading.Tasks.Task DirAllFilesDirectAsync(string Platform, string[] Files);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileMQServiceChannel : FileTransfer.WinTest.ServiceReference1.IFileMQService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileMQServiceClient : System.ServiceModel.ClientBase<FileTransfer.WinTest.ServiceReference1.IFileMQService>, FileTransfer.WinTest.ServiceReference1.IFileMQService {
        
        public FileMQServiceClient() {
        }
        
        public FileMQServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileMQServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileMQServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileMQServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void FileTransferNotification(string Platform) {
            base.Channel.FileTransferNotification(Platform);
        }
        
        public System.Threading.Tasks.Task FileTransferNotificationAsync(string Platform) {
            return base.Channel.FileTransferNotificationAsync(Platform);
        }
        
        public void TransferAllFilesDirect(string Platform, FileTransfer.WinTest.ServiceReference1.FileTransferBase[] toTransfer) {
            base.Channel.TransferAllFilesDirect(Platform, toTransfer);
        }
        
        public System.Threading.Tasks.Task TransferAllFilesDirectAsync(string Platform, FileTransfer.WinTest.ServiceReference1.FileTransferBase[] toTransfer) {
            return base.Channel.TransferAllFilesDirectAsync(Platform, toTransfer);
        }
        
        public void DirAllFilesDirect(string Platform, string[] Files) {
            base.Channel.DirAllFilesDirect(Platform, Files);
        }
        
        public System.Threading.Tasks.Task DirAllFilesDirectAsync(string Platform, string[] Files) {
            return base.Channel.DirAllFilesDirectAsync(Platform, Files);
        }
    }
}