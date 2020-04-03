$('.Posts').click(function (e) {
    var x = window.location.href;
    var linkList = x.split("/");
    console.log(linkList);
    var link = linkList[0] + "//" + linkList[2] + "/Profile/Like";

    if (e.target.classList.contains("LikeClass")) {
        var parent = e.target;
        for (var i = 0; i < 3; i++) {
            if (parent.classList.contains("uniqueLike")) {
                break;
            }
            parent = parent.parentElement;
        }
        var _userID = parent.getAttribute('data-id');
        var _postID = parent.getAttribute('data-post-id');

        $.ajax({
            type: "POST",
            url: link,
            data: {
                "UserId": _userID,
                "PostId": _postID
            },

            dataType: "text",
            success: function (msg) {
                var color = parent.childNodes[1].style.color;

                if (color == "black") {
                    parent.childNodes[1].style.color = "blue";
                } else {
                    parent.childNodes[1].style.color = "black";
                }
                var color = parent.childNodes[0].style.color;

                if (color == "black") {
                    parent.childNodes[0].style.color = "blue";
                } else {
                    parent.childNodes[0].style.color = "black";
                }
                //console.log(parent.childNodes)
                //if (linkList[4] == "Friend_Profile") {
                    
                //}
                //else {
                    
                //}
            },
            error: function (req, status, error) {
                alert(error);

            }
        });
    }

});

$('.Posts').keydown(function (e) {
    
    var x = window.location.href;
    var linkList = x.split("/");
    console.log(linkList);
    var link = linkList[0] + "//" + linkList[2] + "/Profile/AddComment";
    if (e.target.classList.contains("typeComment")) {
        var _userID = e.target.getAttribute('data-id');
        var _postID = e.target.getAttribute('data-post-id');
        var img = e.target.getAttribute('data-prof-pic');
        var userName = e.target.getAttribute('data-name')
        if (e.keyCode == 13) {
            $.ajax({
                type: "POST",
                url: link,
                data: {
                    "UserId": _userID,
                    "CommentContent": e.target.value,
                    "PostId": _postID
                },

                dataType: "text",
                success: function (msg) {
                    var parentDiv = document.createElement("div");
                    parentDiv.classList.add("row");
                    parentDiv.classList.add("p-0");
                    parentDiv.classList.add("m-0");
                    parentDiv.classList.add("mt-3");
                    parentDiv.classList.add("Commented");
                    parentDiv.id = msg;
                    var firstChild = document.createElement("div");
                    firstChild.classList.add("col-1");
                    firstChild.innerHTML = "<img src=" + img + "  class=\"commentPic\">";
                    parentDiv.appendChild(firstChild);
                    var secondChild = document.createElement("div");
                    secondChild.classList.add("col-3");
                    secondChild.classList.add("mt-1");
                    secondChild.classList.add("commentedNameDiv");
                    secondChild.innerHTML = "<span class=\"commentedName\">" + userName + "</span><br>";
                    parentDiv.appendChild(secondChild);
                    var thirdChild = document.createElement("div");
                    thirdChild.classList.add("col-7");
                    thirdChild.classList.add("p-0");
                    thirdChild.classList.add("CommentText2Div");
                    thirdChild.innerHTML = "<span class=\"CommentText2\">" + e.target.value + "</span>";
                    parentDiv.appendChild(thirdChild);

                    var fourthChild = document.createElement("select");
                    fourthChild.classList.add("RemoveCommentSelect");
                    fourthChild.classList.add("col-1");
                    var fourthoption1 = document.createElement("option");
                    fourthoption1.value = "";
                    fourthoption1.innerHTML = "";

                    var fourthoption2 = document.createElement("option");
                    fourthoption2.value = msg;
                    fourthoption2.classList.add("remove_Comment");
                    fourthoption2.innerHTML = "Remove Comment";

                    fourthChild.appendChild(fourthoption1);
                    fourthChild.appendChild(fourthoption2);


                    parentDiv.appendChild(fourthChild);



                    var theParentDiv = e.target.parentElement.parentElement.parentElement;
                    var commentsDiv = theParentDiv.querySelector('.Comments');
                    console.log(commentsDiv);
                    commentsDiv.appendChild(parentDiv);
                    e.target.value = "";

                },
                error: function (req, status, error) {
                    alert(error);

                }
            });
        }

    }


});


$('.Posts').change(function (e) {
    if (e.target.classList.contains("RemovePost")) {

        var target;
        for (var i = 0; i < e.target.childNodes.length; i++) {
            if (e.target.childNodes[i].tagName == "OPTION" && e.target.childNodes[i].innerHTML != "") {
                target = e.target.childNodes[i];
                var id = target.value;
                var _parent = target.parentElement.parentElement.parentElement.parentElement;
                console.log(_parent);
                $.ajax({
                    type: "POST",
                    url: "../Profile/RemovePost",
                    data: {
                        "PostId": id,
                    },
                    dataType: "text",
                    success: function (msg) {
                        console.log("Success")

                        _parent.style.display = "none";
                    },
                    error: function (req, status, error) {
                        alert("Error Happen " + error);
                    }
                });
            }
        }

    }
    else if (e.target.classList.contains("RemoveCommentSelect")) {
        var x = window.location.href;
        var linkList = x.split("/");
        console.log(linkList);
        var link = linkList[0] + "//" + linkList[2] + "/Profile/RemoveComment";
        var target;
        for (var i = 0; i < e.target.childNodes.length; i++) {
            if (e.target.childNodes[i].tagName == "OPTION" && e.target.childNodes[i].innerHTML != "") {
                target = e.target.childNodes[i];
                var id = target.value;
                //var _parent = target.parentElement.parentElement.parentElement.parentElement;
                //console.log(_parent);
                $.ajax({
                    type: "POST",
                    url: link,
                    data: {
                        "CommentId": id,
                    },
                    dataType: "text",
                    success: function (msg) {
                        console.log("Success")
                        console.log("Success");

                        $("#" + id).css("display", "none");
                    },
                    error: function (req, status, error) {
                        alert("Error Happen " + error);
                    }
                });
            }
        }

    }
        
    });


//$('.RemoveCommentSelect').change(function () {
//        console.log("Here Delete Comment");
//    var id = $(this).val();
//    console.log(id);
//        $.ajax({
//            type: "POST",
//            url: "../Profile/RemoveComment",
//            data: {
//                "CommentId": id,
//            },

//            dataType: "text",
//            success: function (msg) {
//                console.log("Success");

//                $("#" + id).css("display", "none");

//            },
//            error: function (req, status, error) {
//                alert("Error Happen " + error);
//            }
//        });

//    });



$('#form_post_btn').click(function (e) {
 
    var _postText = $("#createPostArea").val();
    //var imgPath = $('input[type=file]').val().split('\\').pop();
    var profile_pic = $('#form_post').attr('data-profile-pic');
    var user_name= $('#form_post').attr('data-user-name');
    var user_id= $('#form_post').attr('data-user-id');
    var filename = $('#inputImage').val().replace(/C:\\fakepath\\/i, '');
    var image_path = "../Images/" + filename;
    var imgHtml = '<img src=' + image_path + ' alt="" class="PostImg">'
    if (filename == "") {
        imgHtml = "";
    }
    console.log(filename);
    $.ajax({
        type: "POST",
        url: "../Profile/ProfilePost",
        data: {
            "newPost": _postText,
            "ImageFile": filename
        },
        dataType: "text",
        success: function (post_id) {
            console.log("Success");
            //$('<div class="createPost2 col-12 p-0 pb-1"><div class="row p-0 m-0" ><img src="' + profile_pic + '" alt="" class="PrfilePic m-2" ><div class="col-9 p-0"><div class="row p-0 m-0 mb-1"><span class="ProfileName">' + user_name + '</span><br></div><div class="row p-0 m-0"><span class="PostTime">now</span><i class="fas fa-users ml-3 mt-1"></i></div></div><div class="postOptions col-1 p-0 ml-2"><Select class="RemovePost"><option value=""></option><option value="' + post_id + '">Remove Post</option></Select></div></div><div class="row p-0 m-0 mt-1"><p class="PostText col-12">' + _postText + '<img src="' + image_path + '"  alt="" class="PostImg" ></p><hr class="col-11 m-0 p-0 ml-3" /></div><div class="row p-0 m-0 mt-3"><div class="col-12"><a href="#" class="ListOfLikes">0 Likes</a></div><hr class="col-11 ml-3 mt-3 p-0 m-0" /></div><div class="Like col-6 pt-3 pb-3 LikeClass" data-id="' + user_id + '" data-post-id="' + post_id + '"><span class="row p-0 m-0 Like" style="color:black"><i class="far fa-thumbs-up offset-4 col-1 p-0 mt-1"></i><span class="row p-0 m-0 Like LikeWord">Like</span></span></div><div class="Comment col-6 pt-3 pb-3"><span class="row p-0 m-0"><i class="far fa-comments offset-4 col-1 p-0 mt-1"></i><span class="col-2 p-0">Comment</span></span></div><hr class="col-12 p-0 m-0"></div><div class="row p-0 m-0 mt-3"><div class="col-1"><img src="' + profile_pic + '" alt="" class="commentPic mt-1"></div><div class="col-11 p-0"><textarea id="commentTextArea" placeholder="Type Your Comment Here" class="typeComment" data-prof-pic="' + profile_pic + '" data-name="' + user_name + '" data-id="' + user_id + '" data-post-id="' + post_id + '"></textarea></div></div></div>').insertAfter('.createPost');


            $('<div class="createPost2 col-12 p-0 pb-1">' +
                '<div class="row p-0 m-0">' +
                '<img src="' + profile_pic + '" alt="" class="PrfilePic m-2">' +
                '<div class="col-9 p-0">' +
                '<div class="row p-0 m-0 mb-1">' +
                '<span class="ProfileName">' + user_name + '</span><br>' +
                '</div>' +
                '<div class="row p-0 m-0">' +
                '<span class="PostTime">now</span>' +
                '<i class="fas fa-users ml-3 mt-1"></i>' +
                '</div>' +
                '</div>' +
                '<div class="postOptions col-1 p-0 ml-2">' +
                '<Select class="RemovePost">' +
                '<option value=""></option>' +
                '<option value="' + post_id + '">Remove Post</option>' +
                '</Select>' +
                '</div>' +
                '</div>' +

                '<div class="row p-0 m-0 mt-1">' +
                '<p class="PostText col-12">' +
                _postText +
                imgHtml +

                '</p>' +
                '<hr class="col-11 m-0 p-0 ml-3" />' +
                '</div>' +

                '<div class="row p-0 m-0 mt-3">' +
                '<div class="col-12">' +
                '<a href="#" class="ListOfLikes">Sara Atef Liked This And 15 Other</a>' +
                '</div>' +
                '<hr class="col-11 ml-3 mt-3 p-0 m-0" />' +
                '</div>' +

                '<div class="ListOfPosts row p-0 m-0">' +
                '<div class="Like col-6 pt-3 pb-3 LikeClass uniqueLike" data-id="' + user_id + '" data-post-id="' + post_id + '">' +

                '<span class="row p-0 m-0 Like LikeClass" style="color:black">' +
                '<i class="far fa-thumbs-up offset-4 col-1 p-0 mt-1 LikeClass"></i>' +
                '<span class="row p-0 m-0 Like LikeWord LikeClass">Like</span>' +
                '</span>' +

                '</div>' +
                '<div class="Comment col-6 pt-3 pb-3">' +
                '<span class="row p-0 m-0">' +
                '<i class="far fa-comments offset-4 col-1 p-0 mt-1"></i>' +
                '<span class="col-2 p-0">Comment</span>' +
                '</span>' +
                '</div>' +
                '<hr class="col-12 p-0 m-0">' +
                '</div>' +

                '<div class="Comments">' +

                '</div>' +
                '<div class="row p-0 m-0 mt-3">' +
                '<div class="col-1">' +
                '<img src="' + profile_pic + '" alt="" class="commentPic mt-1">' +
                '</div>' +
                '<div class="col-11 p-0">' +
                '<textarea id="commentTextArea" placeholder="Type Your Comment Here" class="typeComment" data-prof-pic="' + profile_pic + '" data-name="' + user_name + '" data-id="' + user_id + '" data-post-id="' + post_id + '"></textarea>' +
                '</div>' +
                '</div></div>').insertAfter(".createPost");

            $("#createPostArea").val("");

        },
        error: function (req, status, error) {
            alert("Error Happen " + error);
        }
    });
});