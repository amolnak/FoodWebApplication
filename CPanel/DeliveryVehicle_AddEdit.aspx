<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="DeliveryVehicle_AddEdit.aspx.cs" Inherits="CPanel_DeliveryVehicle_AddEdit" %>

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
                                        <label class="control-label">Vehicle No <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="form-control" placeholder="Enter vehicle no." MaxLength="25" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_VehicleNo" runat="server" ControlToValidate="txtVehicleNo" ErrorMessage="Please enter vehicle no."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Brand <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" placeholder="Enter brand." MaxLength="90" TabIndex="2" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Model<span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" placeholder="Enter model." MaxLength="50" TabIndex="3" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Chasis No <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtChasisNo" runat="server" CssClass="form-control" placeholder="Enter chasis no." MaxLength="50" TabIndex="4" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtChasisNo" ErrorMessage="Please enter chasis no."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Fuel Type <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtFuelType" runat="server" CssClass="form-control" placeholder="Enter fuel type." MaxLength="50" TabIndex="5" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">GPS Enabled <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-12 col-md-8">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbYes" runat="server" Text="Yes" GroupName="Status" Checked="true" TabIndex="6" />
                                        </div>
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbNo" runat="server" Text="No" GroupName="Status" TabIndex="7" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/DeliveryVehicleList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="8">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfVehicleID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
