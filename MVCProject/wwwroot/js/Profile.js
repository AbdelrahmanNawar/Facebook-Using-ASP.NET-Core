$("#FriendsDiv").click(function (e) {
	if (e.target.tagName == "BUTTON") {
		if (e.target.innerText.includes("Unfriend")) {
			$.ajax({
				type: "GET",
				url: `../FriendRequests/RemoveFriend/${e.target.id}`,
				success: function () {
					$("#FriendsDiv")[0].removeChild(e.target.parentElement.parentElement);
				},
				error: function (req, status, error) {
					alert("Try again later please!");
				}
			});
		}
		else if (e.target.innerText.includes("Respond")) {
			$.ajax({
				type: "GET",
				url: `../FriendRequests/AcceptFriend/${e.target.id}`,
				success: function () {
					e.target.classList.remove("btn-primary");
					e.target.classList.add("btn-danger");
					e.target.innerHTML = "";
					e.target.innerHTML = "<i class=\"fas fa-user-minus\"></i> Unfriend";
				},
				error: function (req, status, error) {
					alert("Try again later please!");
				}
			});
		}
	}
});

$(".typeComment").keydown(function () {
	var el = this;
	setTimeout(function () {
		el.style.cssText = 'height:auto; padding:0';
		// for box-sizing other than "content-box" use:
		// el.style.cssText = '-moz-box-sizing:content-box';
		el.style.cssText = 'height:' + el.scrollHeight + 'px';
	}, 0);
});

//$('input[type=file]').change(function () {
//	console.log(this.files[0].mozFullPath);
//});
