
$(function () {
    $("#Search").click(function () {
        if ($("#Search").val() == "") {
            alert("Please Select a value before Search");
            return false;
        }
    });
    $("#ShowAll").click(function () {
        if ($("#Search").val() != "") {
            alert("Please do not enter a value for search");
            return false;
        }
    });
});

     function PagerClick(index) {
        document.getElementById("hfCurrentPageIndex").value = index;
        alert(index);
        document.forms[1].submit();
    }
    function GetCurrIndex() {
        document.getElementById("hfCurrentPageIndex").value = index;
        alert(index);
        return index;
    }

    function getMyPid() {
        if ($("input[name='rbtPid']:checked").length > 0) {return $("input[name='rbtPid']:checked").val();}
        else {return false;}
    }
