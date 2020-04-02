$("document").ready(function () {

    $('.new_Btn').bind("click", function () {
        $('#html_btn').click();
    });

    //PostButton --- Publish Post
    $(".btn-Post").click(function () {

        var _postText = $("#createPostArea").val();
        var userId = $(this).attr('data-UserId');
        var userFullName = $(this).attr('data-UserFullName');
        console.log(userId);

        if (_postText != "") {
            $.ajax({
                type: "POST",
                url: "Home/Create",
                data: {
                    "PostContent": _postText,
                    "UserId": userId,
                    "IsDeleted": false
                },

                dataType: "text",
                success: function (postId) {
                    $("#createPostArea").val("");
                    $(' <div class="createPost2" data-id=' + postId + '><img src="../Images/prof.png" alt="" class="PrfilePic" ><span class="ProfileName"> ' + userFullName + '</span> <br><span class="PostTime">now</span> <i class="fas fa-users"></i><Select><option value=""></option><option value="" class="RemovePost">Remove Post</option></Select><p class="PostText">' + _postText + '</p><div class="react"><span class="Like" data-id=' + postId + '><i class="far fa-thumbs-up"></i><span class="LikeWord">Like</span></span><span class="Comment"><i class="far fa-comments"></i><span>Comment</span></span></div><hr><div class="comments"><div class="pComment"><div class="CommingComment"><img src="../Images/prof.png" alt="" class="commentPic"><textarea name="" id="areaOfComment" placeholder="Type Your Comment Here" class="typeComment" data-id=' + postId +'></textarea></div></div></div></div>').insertAfter('.createPost');
                },
                error: function (req, status, error) {
                    alert("Error Happen " + error);
                }
            });
        }
    });
 
    $(".ListOfPosts").click(function (e) {

        if (e.target.classList.contains('LikeWord')) {
            var _that = $(event.target).parent();
            var _postID = _that.attr('data-PostId');
            var _userID = _that.attr('data-UserId');

            $.ajax({
                type: "POST",
                url: "Home/Like",
                data: {
                    "PostId": _postID,
                    "IsLiked": true,
                    "UserId": _userID,
                },

                dataType: "text",
                success: function (msg) {
                    var color = _that.children().css("color");
                    if (color == "rgb(0, 0, 0)") {
                        color = "rgb(0,0,1)";
                        _that.children().css("color", "blue");
                    } else {
                        color = "rgb(0, 0, 0)";
                        _that.children().css("color", "black");
                    }
                },
                error: function (req, status, error) {
                    alert("Error Happen " + error);
                }
            });
        }
    });


    $(".ListOfPosts").keydown(function (e) {
        var _that = $(event.target);
        var _commentText = $(_that).val();

        if (e.target.classList.contains('typeComment')) {
            if (e.keyCode == 13) {
                var _PostID = _that.attr('data-PostId');
                var _UserID = _that.attr('data-UserId');
                var _userFullName = _that.attr('data-UserFullName');
                
                if (_commentText != "") {
                    $.ajax({
                        type: "POST",
                        url: "Home/AddComment",
                        data: {
                            "CommentContent": _commentText,
                            "UserId": _UserID,
                            "IsDeleted": false,
                            "PostId": _PostID
                        },
                        dataType: "text",
                        success: function (CommentId) {
                            $('.typeComment').val(" ");
                            var _insertVarElement = _that.parent().parent();
                            $('<div class="commented" id=' + CommentId + '><img src = "../Images/prof.png" alt = "" class= "commentPic" ><p class="commenttext"><span class="CommentedName">' + _userFullName + '</span><br><span><span><Select class="RemoveComment"><option value=""></option><option value=' + CommentId +' class="RemoveComment">Remove Comment</option></Select><span class="CommentText">' + _commentText+'</span> </p></div>').insertAfter(_insertVarElement);
                        },
                        error: function (req, status, error) {
                            alert("Error Happen " + error);
                        }
                    });
                }
            }
        }
    });

    $('.RemovePost').change(function () {

        console.log("Here Delete");
        var id = $(this).val();
        var _parent = $(this).parent();
        $.ajax({
            type: "POST",
            url: "Home/RemovePost",
            data: {
                "postId": id,
            },
            dataType: "text",
            success: function (msg) {
                console.log("Success")
                _parent.css("display", "none");
            },
            error: function (req, status, error) {
                alert("Error Happen " + error);
            }
        });


    });

    $('.RemoveComment').change(function () {
        console.log("Here Delete Comment");
        var id = $(this).val();

        $.ajax({
            type: "POST",
            url: "Home/RemoveComment",
            data: {
                "commentId": id,
            },
            dataType: "text",
            success: function (msg) {
                console.log("Success");
                $("#" + id).css("display", "none");
            },
            error: function (req, status, error) {
                alert("Error Happen " + error);
            }
        });
    });
});



