﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileTransfer.WCFUpload.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IFileTransferService")]
    public interface IFileTransferService {
        
        // CODEGEN: Generating message contract since the wrapper name (FileTransferRequest) of message FileTransferRequest does not match the default value (Upload)
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileTransferService/Upload")]
        void Upload(FileTransfer.WCFUpload.ServiceReference1.FileTransferRequest request);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFileTransferService/Dir")]
        void Dir(string[] files);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FileTransferRequest", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class FileTransferRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FileName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string Platform;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.IO.Stream Data;
        
        public FileTransferRequest() {
        }
        
        public FileTransferRequest(string FileName, string Platform, System.IO.Stream Data) {
            this.FileName = FileName;
            this.Platform = Platform;
            this.Data = Data;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileTransferServiceChannel : FileTransfer.WCFUpload.ServiceReference1.IFileTransferService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileTransferServiceClient : System.ServiceModel.ClientBase<FileTransfer.WCFUpload.ServiceReference1.IFileTransferService>, FileTransfer.WCFUpload.ServiceReference1.IFileTransferService {
        
        public FileTransferServiceClient() {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void FileTransfer.WCFUpload.ServiceReference1.IFileTransferService.Upload(FileTransfer.WCFUpload.ServiceReference1.FileTransferRequest request) {
            base.Channel.Upload(request);
        }
        
        public void Upload(string FileName, string Platform, System.IO.Stream Data) {
            FileTransfer.WCFUpload.ServiceReference1.FileTransferRequest inValue = new FileTransfer.WCFUpload.ServiceReference1.FileTransferRequest();
            inValue.FileName = FileName;
            inValue.Platform = Platform;
            inValue.Data = Data;
            ((FileTransfer.WCFUpload.ServiceReference1.IFileTransferService)(this)).Upload(inValue);
        }
        
        public void Dir(string[] files) {
            base.Channel.Dir(files);
        }
    }
}
