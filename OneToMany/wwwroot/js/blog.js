$(function () {

    $(document).on("click", ".show-more button", function () {
        //row daxilindeki bloglarin sayini qaytarir
        let skip = parseInt($(".blogs-area").children().length);

        let blogsCount = parseInt($(".blogs-area").attr("data-count"))

        let parentElem = $(".blogs-area");

        let parentElemContent = $(".blogs-area").html();


        $.ajax({
            url: `blog/showmore?skip=${skip}`,
            type:'GET',
            success: function (response) {

                parentElemContent += response;

                $(parentElem).html(parentElemContent)

                let skip = parseInt($(".blogs-area").children().length);

                if (skip >= blogsCount) {
                    $(".show-more button").addClass("d-none")
                }

                
            }
        })
            
    })
 
})


//$(document).on("click", ".show-more button", function () {
//    $.ajax({
//        url: "blog/showmore",
//        type: 'GET',
//        success: function (response) {
//        }
//    })