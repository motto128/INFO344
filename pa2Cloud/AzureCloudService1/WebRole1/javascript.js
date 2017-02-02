
$(document).ready(function() {
    $('input').keyup(function () {
        $("#results").empty();
        var userSearch = $(this).val();
        $.ajax({
            url: "getQuerySuggestions.asmx/searchTrie",
            type: "POST",
            dataType: "json",
            data: JSON.stringify({ input: userSearch }),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $("#results").empty();
                $("#none").empty();
                var resultArray = JSON.parse(result.d);
                display(resultArray, userSearch);
            }
        });
    });
});


function display(result, input)
{
    if (result.length == 0 && input.trim() != "") {
        $("#none").append($("<p></p>").append("No suggestions found for: " + input));
    }
    else
    {
        for (var i = 0; i < result.length; i++) 
        {
            var word = result[i].replace(/_/g, " ");
            var tr = "<tr class='table_rows'>";
            var td = "<td>" + word + "</td></tr>";
            $("#results").append(tr + td);
        }
    }
}