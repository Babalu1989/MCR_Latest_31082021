<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="frmInstallerOnMap.aspx.cs" Inherits="frmInstallerOnMap" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #installerLocOnMap {
            height: 540px;
        }
    </style>

    <script type="text/javascript"
        src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBsqjUz1fqh8lRECJ_851-kCS0xb4VoFnk">  
    </script>
    <script type="text/javascript"
        src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js">
    </script>

    <script type="text/javascript">  
        var map;
        function initialize() {
            var mapOptions = {
                center: new google.maps.LatLng(28.63610913, 77.18402534),
                zoom: 20,
                mapTypeId: google.maps.MapTypeId.TERRAIN
            };
            map = new google.maps.Map(document.getElementById("installerLocOnMap"),
                mapOptions);

        }
        google.maps.event.addDomListener(window, 'load', initialize);

    </script>

    <%--Load User List Details--%>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "POST",
                url: "frmInstallerOnMap.aspx/BindUserDetails",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var ddlCustomers = $("[id*=txtDivision]");
                    ddlCustomers.empty().append('<option selected="selected" value="0">Please select</option>');
                    $.each(data.d, function (key, value) {
                        //alert(value.UserID + value.UserName);
                        ddlCustomers.append($("<option></option>").val(value.UserID).html(value.UserName));

                    });
                },
                error: function (result) {
                    alert("Not able to load userlist");
                }
            });
        });
    </script>

    <%--Lat and Long for User Selected--%>
    <script type="text/javascript">       

        $(function () {
            $("[id*=btnSubmit]").click(function () {
                var userId = $.trim($("[id*=txtDivision]").val());
                var date = $.trim($("[id*=txtFromDate]").val());

                var flightPlanCoordinatesNew = [];
                var ContentForEachNew = [];
                
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmInstallerOnMap.aspx/BindLatLongDetails",
                    data: JSON.stringify({ 'strUserId': userId, 'strDate': userId }),
                    dataType: "json",
                    success: function (data) {
                        if (data.d.length > 0) {

                            $.each(data.d, function (key, value) {
                                //alert(value.UserID);
                                //$("#cons-name").text(value.UserID);
                                //$("#cons-mbno").text(value.Latitude);
                                //$("#cons-address").text(value.Longitude);
                                //$("#cons-pincode").text(value.DateTime);

                                var contentString = '<div id="content">' +
                                    '<div id="siteNotice">' +
                                    '</div>' +
                                    //'<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
                                    '<div id="bodyContent">' +
                                    '<p><b>Installer Movement</b></p>' +
                                    '<p>UserId : ' + value.UserID +'</p></br>' +                                    
                                    '<p>Date : </p>' + value.DateTime +'</br>' +                                    
                                    //'<p></p></br>' +                                    
                                    '</div>' +
                                    '</div>';

                                clearMap();
                                flightPlanCoordinatesNew.push(new google.maps.LatLng(value.Latitude, value.Longitude));
                                ContentForEachNew.push(contentString);
                                initializeNew(flightPlanCoordinatesNew, ContentForEachNew);

                            });
                        } else {
                            clearMap();
                            alert("No record found.");
                        }
                    },
                    error: function (result) {
                        alert("Error");
                    }
                });
                return false;
            });
        });
    </script>

    <%--Draw Lat & Long for User selected--%>
    <script type="text/javascript">
        var mapNew;
        function initializeNew(flightPlanCoordinatesNew, ContentForEachNew) {
            var mapOptions = {
                center: new google.maps.LatLng(28.63610913, 77.18402534),
                zoom: 20,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            mapNew = new google.maps.Map(document.getElementById("installerLocOnMap"),
                mapOptions);
            
            var lineSymbol = {
                path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
            };
            
            var map;
            var polyline;

            var markers = flightPlanCoordinatesNew;
            var contentString = ContentForEachNew;
            
            var infowindow = new google.maps.InfoWindow({
                //content: contentString
                maxWidth: 200
            });

            var polylineoptns = {
                strokeColor: '#009688',
                strokeOpacity: 1.0,
                strokeWeight: 5,
                geodesic: true,
                map: mapNew,
                icons: [{
                    repeat: '200px', //CHANGE THIS VALUE TO CHANGE THE DISTANCE BETWEEN ARROWS
                    icon: lineSymbol,
                    offset: '100%'
                }]
            };
            polyline = new google.maps.Polyline(polylineoptns);
            var z = 0;
            var path = [];
            path[z] = polyline.getPath();
            for (var i = 0; i < markers.length; i++) //LOOP TO DISPLAY THE MARKERS
            {
                var pos = markers[i];
                var content = contentString[i];
                var marker = new google.maps.Marker({
                    position: pos,
                    map: mapNew,
                    title: 'Installer Movements'
                });
                path[z].push(marker.getPosition()); //PUSH THE NEWLY CREATED MARKER'S POSITION TO THE PATH ARRAY
                marker.addListener('click', function () {
                    infowindow.setContent(content);
                    infowindow.open(map, marker);
                });
            }
        }

        function clearMap() {
            var mapOptions = {
                center: new google.maps.LatLng(28.63610913, 77.18402534),
                zoom: 20,
                mapTypeId: google.maps.MapTypeId.TERRAIN
            };
            map = new google.maps.Map(document.getElementById("installerLocOnMap"),
                mapOptions);
        }        

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="SM1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div>
        <table width="100%" style="border: 1px solid black;">
            <%--<tr>
                <td colspan="4" style="height: 15px"></td>
            </tr>--%>
            <tr>
                <td>Name
                </td>
                <td>
                    <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea">
                    </asp:DropDownList>
                </td>
                <td>Date
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"></asp:TextBox>
                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                </td>
                <td>
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" />
                </td>
            </tr>
            <%--<tr>
                <td colspan="4" style="height: 15px"></td>
            </tr>--%>
        </table>
    </div>

    <div id="installerLocOnMap"></div>
</asp:Content>

