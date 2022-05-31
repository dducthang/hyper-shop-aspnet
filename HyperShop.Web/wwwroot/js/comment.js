$(document).ready(function () {
    loadComments()
});


//---------comment---------------

//post comment
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
        },
        error: function (err) {
            if (err.status = 401)
                Swal.fire({
                    icon: 'error',
                    text: 'Please login!'
                });
            else Swal.fire({
                icon: 'error',
                text: 'Something wrong'
            });
        }
    })


})

//comment paging
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

//-------------------------------

//---------response--------------
$('.comments-list').on('submit', '.response-form', function (e) {
    e.preventDefault();
    e.stopPropagation();
    const form = $(this);
    const data = form.serialize();
    const url = form.attr('action');

    $.ajax({
        url,
        data,
        method: 'post',
        success: function (data) {
            form.prev('.responses').append(getNewResponse(data))
            $('.rep-comment').val('');
        },
        error: function (err) {
            if (err.status = 401)
                Swal.fire({
                    icon: 'error',
                    text: 'Please login!'
                });
            else Swal.fire({
                icon: 'error',
                text: 'Something wrong'
            });
        }
    })
})

//-------------------------------




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
            console.log(err)

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
      </div>`
        res += getResponses(comment.responses);
        res += `
    <form class="response-form" action="/customer/response/post" method="post" autocomplete="off">
      <input type="text" class="rep-comment" name="body" placeholder="Reply comment .....">
      <input hidden name="commentId" value="${comment.id}">
      <button type="submit" hidden></button>
    </form>
      <div class="divide"></div>
    </div>
    `

    }

    return res
}


function getResponses(responses) {
    let res = `<div class = "responses">`;
    for (response of responses) {
        res += `
     
          <div class="response">
        <div class="arrow"></div>
        <div class="customer-avatar">
            <i class="far fa-user-circle medium-text"></i>&nbsp; &nbsp;<span>${response.applicationUser.fullName}</span>
        </div>
        <div class="small-text black-color">
            ${response.body}
        </div>
    </div>
      
    `
    }
    res += `</div>`;

    return res
}

function getNewResponse(response) {
    return`
     
          <div class="response">
        <div class="arrow"></div>
        <div class="customer-avatar">
            <i class="far fa-user-circle medium-text"></i>&nbsp; &nbsp;<span>${response.applicationUser.fullName}</span>
        </div>
        <div class="small-text black-color">
            ${response.body}
        </div>
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
