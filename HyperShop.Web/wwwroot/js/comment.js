$(document).ready(function () {
    loadComments()
});


$('.comment-form').on('submit', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const body = $('.comment-input').val();
    if (body == '') return;
    const productId = $(this).attr('id');
    const url = $(this).attr('action');

    $.ajax({
        url,
        method: "post",
        data: {
            body,
            productId
        },
        success: function (data) {
            page = 1;
            loadComments();
            $('.comment-input').val('');
        }
    })


})

$('.pages').on('click', '.page-item', function (e) {
    e.preventDefault();
    e.stopPropagation();
    let newPage;
    //check if page is current page 


    if ($(this).text() == 'First') newPage = 1;
    else if ($(this).text() == 'Last') newPage = lastPage;
    else
        newPage = $(this).text();
    if (newPage != page) {
        page = newPage;
        loadComments();
    }
})



//utility
function loadComments() {
    $.ajax({
        url: "/Customer/Comment/Get",
        traditional: true,
        data: {
            page,
            taking,
            productId
        },
        dataType: "json",
        success: function (res) {
            lastPage = Math.ceil(res.commentCount / taking);
            $('.comments-list').html(getCommentList(res.comments));
            $('.pages').html(getPagesNumber(lastPage, page));

                        
        },
        error: function (err) {
            console.log("failed")

        }
    })
}

function getCommentList(comments) {
    let res = ``;
    for (comment of comments) {
        res += `
     <div class="comment">
      <div class="customer-avatar">
        <i class="far fa-user-circle medium-text"></i>&nbsp; &nbsp;<span>${comment.applicationUser.fullName}</span>
      </div>
      <div class="comment-content">
        <span class="light-gray-color">${comment.body}</span>
      </div>
    <form class="response-form" action="/api/response/" method="post" autocomplete="off">
      <input type="text" class="rep-comment" name="response" placeholder="Reply comment .....">
      <button hidden></button>
    </form>
      <div class="divide"></div>
    </div>
    `
    }

    return res
}

function getComment(comment) {
    return`
     <div class="comment">
      <div class="customer-avatar">
        <i class="far fa-user-circle medium-text"></i>&nbsp; &nbsp;<span>${comment.applicationUser.fullName}</span>
      </div>
      <div class="comment-content">
        <span class="light-gray-color">${comment.body}</span>
      </div>
    <form class="response-form" action="/api/response/" method="post" autocomplete="off">
      <input type="text" class="rep-comment" name="response" placeholder="Reply comment .....">
      <button hidden></button>
    </form>
      <div class="divide"></div>
    </div>
    `
}

function getPagesNumber(lastPage, page) {
    let res = `
<nav
  aria-label="Page navigation example"
  class="d-flex justify-content-center"
>`;
    if (lastPage > 0) {
        res += `<ul class="pagination"><li class="page-item"><a href="#" class="page-link">First</a></li>`;
        let i = Number(page) > 5 ? Number(page) - 4 : 1;
        if (i !== 1) {
            res += `<li class="page-item"><a href="#" class="page-link">...</a></li>`;
        }
        for (; i <= Number(page) + 4 && i <= lastPage; i++) {
            if (i == page) {
                res += `<li class="page-item active"><a href="#" class="page-link">${i}</a></li>`;
            } else {
                res += `<li class="page-item"><a href="#" class="page-link">${i}</a></li>`;
            }
            if (i == Number(page) + 4 && i < lastPage) {
                res += `<li class="page-item"><a href="#" class="page-link">...</a></li>`;
            }
        }
        res += `<li class="page-item"><a href="#" class="page-link">Last</a></li>`;
        res += `</ul>`;
    }

    res += `</nav>`;
    return res;
}
