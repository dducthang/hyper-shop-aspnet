$('.comment-form').on('submit', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const body = $('.comment-input').val();
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
            $('.comments-list').append(renderComment(data));
        }
    })


})

function renderComment(comment) {
    return `
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