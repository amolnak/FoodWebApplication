<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="City_AddEdit.aspx.cs" Inherits="CPanel_City_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
       
        <div class="row">
             <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                       <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                 
                        <div class="row">
                             
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">City Code <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtCityCode" runat="server" CssClass="form-control" placeholder="Enter city code" MaxLength="3" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_citycode" runat="server" ControlToValidate="txtCityCode" ErrorMessage="Please enter city code."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">City Name <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-10 col-md-6">
                                        <asp:TextBox ID="txtCityName" runat="server" CssClass="form-control" placeholder="Enter city name" MaxLength="90" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_cityname" runat="server" ControlToValidate="txtCityName" ErrorMessage="Please enter city name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Status <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-12 col-md-8">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbActive" runat="server" Text="Active" GroupName="Status" Checked="true" TabIndex="3" />
                                        </div>
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbInActive" runat="server" Text="Inactive" GroupName="Status" TabIndex="4" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/CityList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="5">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfCityID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
