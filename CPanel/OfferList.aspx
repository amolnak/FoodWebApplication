﻿<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="OfferList.aspx.cs" Inherits="CPanel_OfferList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
        <link href="../css/jqgrid/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" type="text/css" media="screen" href="../css/jqgrid/ui.jqgrid.css" />
        <script type="text/javascript" src="../js/1.11.0/jquery.min.js"></script>
        <div class="row">
            <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                        <div align="center">
                            <strong>Manage Offers </strong>
                        </div>
                        <div style="padding-top: 5px;" align="right">
                            <a href="Offer_AddEdit.aspx" target="_parent" class="btn btn-link btn-sm"><strong>Add Offer <i class="fa fa-plus"></i></strong></a>
                        </div>
                        <div style="clear: both; padding: 2px;"></div>

                        <div class="clear"></div>
                        <table id="theGrid" runat="server"></table>
                        <div id="gridPager" runat="server"></div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="Dashboard1.aspx" class="btn btn-secondary btn-sm"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="ltrScript" runat="server" />

        <script type="text/javascript">
            $(document).ready(function () {
                $('#jqgh_ContentPlaceHolder1_theGrid_Active').css("text-align", "center");
                $('#jqgh_ContentPlaceHolder1_theGrid_AdminId').css("text-align", "center");
            });

            function DeleteOffer(OfferID) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to delete this offer?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    closeOnConfirm: false
                },
           function () {
               PageMethods.DeleteOffer(OfferID, OnSuccessDelOffer, OnErrorDelOffer);
           });

            }

            function OnSuccessDelOffer(result) {
                if (result != "") {
                    refreshGrid('#<%=theGrid.ClientID%>', JSON.parse(result));
               swal({
                   title: "",
                   text: "Offer deleted successfully.",
                   type: "success",
                   confirmButtonText: "OK",
                   closeOnConfirm: false
               }, function (isConfirm) {
                   if (isConfirm) {
                       window.location.reload();
                   }
               });
           }
           else {
                    swal("", "Error occurred while deleting offer.", "error");
           }
       }

       function OnErrorDelOffer(result) {
           swal("", "Error occurred while deleting offer.", "error");
       }

       function refreshGrid(grid, results) {
           var objres = (results);
           $(grid).jqGrid('clearGridData')
               .jqGrid('setGridParam', { data: objres })
               .trigger('reloadGrid', [{ page: 1 }]);
       }
        </script>
    </form>
</asp:Content>
