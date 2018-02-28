function SearchDiscountTypeByString()
{
    $(function () {
        var pidvalue = 0;
        $("#Search").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/GeneralMaster/SearchDiscountTypeByString",
                    type: "POST",
                    dataType: "json",
                    data: { prefix: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            pidvalue = item.Pid;
                            return { value: item.DiscountTypeName, label: item.DiscountTypeName };
                        }));
                    }, error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("#Pid").val(pidvalue);
            },
            minLength: 3,
            messages: {
                noResults: "", results: function (resultsCount) { }
            }
        });
    });
}


    function checkrights(pagename, linkname, rightheader) {
        alert("CheckUserRights");
        var pid = getMyPid();
        if (pid === false) {
            alert("Pid is false");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "CheckUserGroupRights",//'@Url.Action("CheckUserGroupRights")',
            contentType: "application/json; charset=utf-8",
            dataType: "JSON",
            data: '{pagename: "' + pagename + '" ,linkname: "' + linkname + '",rightheader: "' + rightheader + '"   }',
            success:
            function (data) {
                if (data === "YES") {
                    alert("You Have rights");
                    switch (rightheader) {
                        //case "ActionDiscountType": window.location.href = '@Url.Action("ActionDiscountType", "GeneralMaster")';
                        case "ActionDiscountType": window.location.href =  rightheader;
                        break;

                        //case "EditDiscountType": window.location.href = '@Url.Action("EditDiscountType", "GeneralMaster",new {pid="xxx"})'.replace("xxx", pid);
                        case rightheader: window.location.href = rightheader + "?pid=" + pid;
                        break;

                        //case "DeleteDiscountType": var url = '@Url.Action("DeleteDiscountType", "GeneralMaster")';
                        //    url += '?pid=' + pid;
                        //    window.location.href = url;
                        //    break;
                    }
                }
                else { alert("You do not have rights---No Data"); }
            },
            error: onerror
        });
        //@*function Onsuccess() {
        //    alert("Hi");
        //    window.location.href = '@Url.Action("DiscountType", "GeneralMaster",new {@Link="DiscountType",@Mode="Get"})';
        //} *@
        function onerror() {
            alert("You do not have rights---Error");
        }
    }
