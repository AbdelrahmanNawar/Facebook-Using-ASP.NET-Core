$(".blockUserBtn").click(function (e) {
    if (e.target.innerText.includes("Un")) {
        $.ajax({
            type: "GET",
            url: `../Administration/Unblock/${e.target.id}`,
            success: function () {
                e.target.innerHTML = "";
                e.target.classList.remove("btn-primary");
                e.target.classList.add("btn-danger");
                e.target.innerHTML = "<i class=\"fas fa-user-lock \"></i> Block";
            },
            error: function (req, status, error) {
                alert("Try again later please!");
            }
        });
    }
    else {
        $.ajax({
            type: "GET",
            url: `../Administration/Block/${e.target.id}`,
            success: function () {
                e.target.innerHTML = "";
                e.target.classList.remove("btn-danger");
                e.target.classList.add("btn-primary");
                e.target.innerHTML = "<i class=\"fas fa-lock-open \"></i> Unlock";
            },
            error: function (req, status, error) {
                alert("Try again later please!");
            }
        });
    }
});


$('.rolesDropdown').on('change', function (e) {
    var role = e.target.options[e.target.options.selectedIndex].value;
    $.ajax({
        type: "POST",
        url: `../Administration/ChangeRole/${e.target.id}`,
        data: { role: role },
        dataType: "text",
        success: function () {
        },
        error: function (req, status, error) {
            alert("Try again later please!");
        }
    });
});


$('#adminSearchText').keydown(function (e) {
    if (e.key === "Enter") {
        $.ajax({
            type: "POST",
            url: `../Administration/AdminUserSearch`,
            data: { searchText: e.target.value },
            dataType: "text",
            success: function (data) {
                $("#adminUsersList").html(data);
            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    }
    
});
