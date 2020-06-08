<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="Item_AddEdit.aspx.cs" Inherits="CPanel_Item_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
        <div class="row">
            <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                        <div align="center">
                            <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                        </div>

                        <div class="row mt-20">
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Code <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control" placeholder="Enter item code." MaxLength="15" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_iemcode" runat="server" ControlToValidate="txtItemCode" ErrorMessage="Please enter item code."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Name <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" placeholder="Enter item name." MaxLength="100" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_itemname" runat="server" ControlToValidate="txtItemName" ErrorMessage="Please enter item name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Type<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="3" />
                                        <asp:RequiredFieldValidator ID="reqfld_itemtype" runat="server" InitialValue="" ControlToValidate="ddlItemType" ErrorMessage="Please select item type."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Description  
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtItemDesc" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" placeholder="Enter item description." MaxLength="500" TabIndex="4" />

                                    </div>
                                </div>
                                <%-- <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Image<span id="spnUpdImg" runat="server" class="text-danger"> </span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <input name="ImageName" type="file" id="ImageName" class="form-control" runat="server" onkeypress="return false" onkeydown="return false" tabindex="5" onkeyup="return false" />
                                        <asp:HyperLink runat="server" ID="ViewImg" Visible="false" Target="_blank" CssClass="Small" />
                                        <div class="clear mt-5"></div>
                                        <a id="a_view" runat="server" target="_blank" class="shadow disInBlock dvAAThumb vibrate \" style="cursor: pointer;" tabindex="6">
                                            <img src="~/CPanel/images/spacer.gif" id="imgAlbumArtThumb" runat="server" class="imguploadThumb img-thumbnail" />
                                        </a>
                                        <div class="clear mt-5"></div>
                                        <asp:CustomValidator ID="cstmvld_Img" Display="Dynamic"
                                            CssClass="text-danger" ClientValidationFunction="ValidateImage" runat="server" ValidationGroup="vg" />
                                    </div>
                                </div>--%>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Price<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-1 col-md-2 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtItemPrice" runat="server" CssClass="form-control" placeholder="Enter price." MaxLength="8" TabIndex="7" />
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-3 col-xs-4" style="vertical-align:middle;">
                                        <asp:RequiredFieldValidator ID="reqfld_itemprice" runat="server" ControlToValidate="txtItemPrice" ErrorMessage="Please enter item price."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Veg / Non-veg  
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbVeg" runat="server" Text="Veg" GroupName="VN" Checked="true" TabIndex="8" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rdbNonVeg" runat="server" Text="Non-Veg" GroupName="VN" TabIndex="9" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item AddOn<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlItemAddOn" runat="server" CssClass="form-control" TabIndex="10" />
                                        <asp:RequiredFieldValidator ID="reqfld_itemaddon" runat="server" ControlToValidate="ddlItemAddOn" InitialValue="" ErrorMessage="Please select item addon."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Star Recipe  
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbYes" runat="server" Text="Yes" GroupName="SR" Checked="true" TabIndex="11" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rdbNo" runat="server" Text="No" GroupName="SR" TabIndex="12" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/ItemList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="12">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfItemID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
