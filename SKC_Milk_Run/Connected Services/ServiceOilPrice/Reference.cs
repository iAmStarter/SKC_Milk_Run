﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SKC_Milk_Run.ServiceOilPrice {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.pttor.com", ConfigurationName="ServiceOilPrice.OilPriceSoap")]
    public interface OilPriceSoap {
        
        // CODEGEN: Generating message contract since element name Language from namespace http://www.pttor.com is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/CurrentOilPrice", ReplyAction="*")]
        SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse CurrentOilPrice(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/CurrentOilPrice", ReplyAction="*")]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse> CurrentOilPriceAsync(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest request);
        
        // CODEGEN: Generating message contract since element name Language from namespace http://www.pttor.com is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/CurrentOilPriceProvincial", ReplyAction="*")]
        SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse CurrentOilPriceProvincial(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/CurrentOilPriceProvincial", ReplyAction="*")]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse> CurrentOilPriceProvincialAsync(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest request);
        
        // CODEGEN: Generating message contract since element name Language from namespace http://www.pttor.com is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/GetOilPrice", ReplyAction="*")]
        SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse GetOilPrice(SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/GetOilPrice", ReplyAction="*")]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse> GetOilPriceAsync(SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest request);
        
        // CODEGEN: Generating message contract since element name Language from namespace http://www.pttor.com is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/GetOilPriceProvincial", ReplyAction="*")]
        SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse GetOilPriceProvincial(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.pttor.com/GetOilPriceProvincial", ReplyAction="*")]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse> GetOilPriceProvincialAsync(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CurrentOilPriceRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CurrentOilPrice", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequestBody Body;
        
        public CurrentOilPriceRequest() {
        }
        
        public CurrentOilPriceRequest(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class CurrentOilPriceRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Language;
        
        public CurrentOilPriceRequestBody() {
        }
        
        public CurrentOilPriceRequestBody(string Language) {
            this.Language = Language;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CurrentOilPriceResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CurrentOilPriceResponse", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponseBody Body;
        
        public CurrentOilPriceResponse() {
        }
        
        public CurrentOilPriceResponse(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class CurrentOilPriceResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string CurrentOilPriceResult;
        
        public CurrentOilPriceResponseBody() {
        }
        
        public CurrentOilPriceResponseBody(string CurrentOilPriceResult) {
            this.CurrentOilPriceResult = CurrentOilPriceResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CurrentOilPriceProvincialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CurrentOilPriceProvincial", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequestBody Body;
        
        public CurrentOilPriceProvincialRequest() {
        }
        
        public CurrentOilPriceProvincialRequest(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class CurrentOilPriceProvincialRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Language;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Province;
        
        public CurrentOilPriceProvincialRequestBody() {
        }
        
        public CurrentOilPriceProvincialRequestBody(string Language, string Province) {
            this.Language = Language;
            this.Province = Province;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CurrentOilPriceProvincialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CurrentOilPriceProvincialResponse", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponseBody Body;
        
        public CurrentOilPriceProvincialResponse() {
        }
        
        public CurrentOilPriceProvincialResponse(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class CurrentOilPriceProvincialResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string CurrentOilPriceProvincialResult;
        
        public CurrentOilPriceProvincialResponseBody() {
        }
        
        public CurrentOilPriceProvincialResponseBody(string CurrentOilPriceProvincialResult) {
            this.CurrentOilPriceProvincialResult = CurrentOilPriceProvincialResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOilPriceRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOilPrice", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequestBody Body;
        
        public GetOilPriceRequest() {
        }
        
        public GetOilPriceRequest(SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class GetOilPriceRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Language;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public short DD;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public short MM;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public short YYYY;
        
        public GetOilPriceRequestBody() {
        }
        
        public GetOilPriceRequestBody(string Language, short DD, short MM, short YYYY) {
            this.Language = Language;
            this.DD = DD;
            this.MM = MM;
            this.YYYY = YYYY;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOilPriceResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOilPriceResponse", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponseBody Body;
        
        public GetOilPriceResponse() {
        }
        
        public GetOilPriceResponse(SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class GetOilPriceResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetOilPriceResult;
        
        public GetOilPriceResponseBody() {
        }
        
        public GetOilPriceResponseBody(string GetOilPriceResult) {
            this.GetOilPriceResult = GetOilPriceResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOilPriceProvincialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOilPriceProvincial", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequestBody Body;
        
        public GetOilPriceProvincialRequest() {
        }
        
        public GetOilPriceProvincialRequest(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class GetOilPriceProvincialRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Language;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public short DD;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public short MM;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public short YYYY;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Province;
        
        public GetOilPriceProvincialRequestBody() {
        }
        
        public GetOilPriceProvincialRequestBody(string Language, short DD, short MM, short YYYY, string Province) {
            this.Language = Language;
            this.DD = DD;
            this.MM = MM;
            this.YYYY = YYYY;
            this.Province = Province;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOilPriceProvincialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOilPriceProvincialResponse", Namespace="http://www.pttor.com", Order=0)]
        public SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponseBody Body;
        
        public GetOilPriceProvincialResponse() {
        }
        
        public GetOilPriceProvincialResponse(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.pttor.com")]
    public partial class GetOilPriceProvincialResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetOilPriceProvincialResult;
        
        public GetOilPriceProvincialResponseBody() {
        }
        
        public GetOilPriceProvincialResponseBody(string GetOilPriceProvincialResult) {
            this.GetOilPriceProvincialResult = GetOilPriceProvincialResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OilPriceSoapChannel : SKC_Milk_Run.ServiceOilPrice.OilPriceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OilPriceSoapClient : System.ServiceModel.ClientBase<SKC_Milk_Run.ServiceOilPrice.OilPriceSoap>, SKC_Milk_Run.ServiceOilPrice.OilPriceSoap {
        
        public OilPriceSoapClient() {
        }
        
        public OilPriceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OilPriceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OilPriceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OilPriceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.CurrentOilPrice(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest request) {
            return base.Channel.CurrentOilPrice(request);
        }
        
        public string CurrentOilPrice(string Language) {
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest inValue = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequestBody();
            inValue.Body.Language = Language;
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse retVal = ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).CurrentOilPrice(inValue);
            return retVal.Body.CurrentOilPriceResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse> SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.CurrentOilPriceAsync(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest request) {
            return base.Channel.CurrentOilPriceAsync(request);
        }
        
        public System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceResponse> CurrentOilPriceAsync(string Language) {
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest inValue = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceRequestBody();
            inValue.Body.Language = Language;
            return ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).CurrentOilPriceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.CurrentOilPriceProvincial(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest request) {
            return base.Channel.CurrentOilPriceProvincial(request);
        }
        
        public string CurrentOilPriceProvincial(string Language, string Province) {
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest inValue = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.Province = Province;
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse retVal = ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).CurrentOilPriceProvincial(inValue);
            return retVal.Body.CurrentOilPriceProvincialResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse> SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.CurrentOilPriceProvincialAsync(SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest request) {
            return base.Channel.CurrentOilPriceProvincialAsync(request);
        }
        
        public System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialResponse> CurrentOilPriceProvincialAsync(string Language, string Province) {
            SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest inValue = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.CurrentOilPriceProvincialRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.Province = Province;
            return ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).CurrentOilPriceProvincialAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.GetOilPrice(SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest request) {
            return base.Channel.GetOilPrice(request);
        }
        
        public string GetOilPrice(string Language, short DD, short MM, short YYYY) {
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest inValue = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.DD = DD;
            inValue.Body.MM = MM;
            inValue.Body.YYYY = YYYY;
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse retVal = ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).GetOilPrice(inValue);
            return retVal.Body.GetOilPriceResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse> SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.GetOilPriceAsync(SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest request) {
            return base.Channel.GetOilPriceAsync(request);
        }
        
        public System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceResponse> GetOilPriceAsync(string Language, short DD, short MM, short YYYY) {
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest inValue = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.DD = DD;
            inValue.Body.MM = MM;
            inValue.Body.YYYY = YYYY;
            return ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).GetOilPriceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.GetOilPriceProvincial(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest request) {
            return base.Channel.GetOilPriceProvincial(request);
        }
        
        public string GetOilPriceProvincial(string Language, short DD, short MM, short YYYY, string Province) {
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest inValue = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.DD = DD;
            inValue.Body.MM = MM;
            inValue.Body.YYYY = YYYY;
            inValue.Body.Province = Province;
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse retVal = ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).GetOilPriceProvincial(inValue);
            return retVal.Body.GetOilPriceProvincialResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse> SKC_Milk_Run.ServiceOilPrice.OilPriceSoap.GetOilPriceProvincialAsync(SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest request) {
            return base.Channel.GetOilPriceProvincialAsync(request);
        }
        
        public System.Threading.Tasks.Task<SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialResponse> GetOilPriceProvincialAsync(string Language, short DD, short MM, short YYYY, string Province) {
            SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest inValue = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequest();
            inValue.Body = new SKC_Milk_Run.ServiceOilPrice.GetOilPriceProvincialRequestBody();
            inValue.Body.Language = Language;
            inValue.Body.DD = DD;
            inValue.Body.MM = MM;
            inValue.Body.YYYY = YYYY;
            inValue.Body.Province = Province;
            return ((SKC_Milk_Run.ServiceOilPrice.OilPriceSoap)(this)).GetOilPriceProvincialAsync(inValue);
        }
    }
}