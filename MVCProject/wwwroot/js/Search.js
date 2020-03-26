$("#searchResult").click(function (e) {
    if (e.target.tagName == "BUTTON") {
        if (e.target.innerText.includes("Add")) {
            $.ajax({
                type: "GET",
                url: `../FriendRequests/AddFriend/${e.target.id}`,
                success: function () {
                    e.target.innerHTML = "";
                    e.target.innerHTML = "<i class=\"fas fa-ellipsis-h statusButtonIcon\"></i> Pending";
                    e.target.disabled = true;
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
                    e.target.innerHTML = "";
                    e.target.innerHTML = "<i class=\"fas fa-check statusButtonIcon\"></i> Friends";
                    e.target.disabled = true;
                },
                error: function (req, status, error) {
                    alert("Try again later please!");
                }
            });
        }
    }
});