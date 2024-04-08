const ulList = $(".menu-wrap ul");
const carts = $(".checkout-order");
const cart_float = $("#cart-float");
function updateCartCount(itemCount) {
    const cartBadges = $(".lblCartCount");
    const order = $("#cart-checkout h6");
    cartBadges.text(itemCount);
    if (order != null) {
        order.text(itemCount);
    }
}

function updateSubtotal(listItem) {
    const unitPrice = parseFloat(listItem.find(".counter-container h3").text().replace("$", ""));
    const quantity = parseInt(listItem.find(".product-qty").val());
    const subtotal = unitPrice * quantity;
    listItem.find(".price h2").text("$" + subtotal);
}

function updateTotalPrice(cart) {
    let total = 0;
    const items = cart.find(".price-list");

    items.each(function () {
        const subtotal = parseFloat($(this).find(".price h2").text().replace("$", ""));
        total += subtotal;
    });

    $(".totel-price h5, .totel-price h2").text("$" + total.toFixed(2));
}

function addCartItemsListener(customerId, cart, check) {
    // Add event listeners for the closeButton icons
    cart.find(".closeButton").on("click", function () {
        const listItem = $(this).closest(".price-list");
        const productId = parseInt(listItem.data("product"));
        listItem.remove();
        updateTotalPrice(cart);
        updateCartItem(customerId, productId, 0);
        updateCartCount(cart.find(".price-list").length);
    });

    // Add event listeners for the "+" and "-" buttons
    cart.find(".qty-count").on("click", function () {
        const listItem = $(this).closest(".price-list");
        const qtyInput = listItem.find(".product-qty");
        const status = parseInt(listItem.attr("status"));
        let quantity = parseInt(qtyInput.val());
        if (status == 0) {
            if ($(this).data("action") === "add") {
                quantity++;
            } else if ($(this).data("action") === "minus" && quantity > 1) {
                quantity--;
            }
        }
        qtyInput.val(quantity);
        updateSubtotal(listItem);
        updateTotalPrice(cart);
        const productId = parseInt(listItem.data("product"));
        updateCartItem(customerId, productId, quantity);
    });

    // Add event listeners for input quantity from keyboard
    cart.find(".product-qty").on("keypress", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            const listItem = $(this).closest(".price-list");
            const productId = parseInt(listItem.data("product"));
            let quantity = parseInt($(this).val());
            if (isNaN(quantity) || $(this).val().trim() === "") {
                quantity = 0;
                $(this).val(quantity);
            }
            updateSubtotal(listItem);
            updateTotalPrice(cart);
            updateCartItem(customerId, productId, quantity);
        }
    });
}

function createCartItem(customerId, productId, productName, unitPrice, quantity, productImage) {
    const newItem = $(`
          <li class="price-list" data-product="${productId}" status="0">
            <i class="closeButton fa-solid fa-xmark"></i>
            <div class="counter-container">
              <div class="counter-food">
                <img alt="food" src="${productImage}">
                <h4>${productName}</h4>
              </div>
              <h3>$${unitPrice}</h3>
            </div>
            <div class="price">
              <div>
                <h2>$${(unitPrice * quantity)}</h2>
                <span>Sum</span>
              </div>
              <div>
                <div class="qty-input">
                  <button class="qty-count qty-count--minus" data-action="minus" type="button">-</button>
                  <input class="product-qty" type="number" name="product-qty" min="1" value="${quantity}">
                  <button class="qty-count qty-count--add" data-action="add" type="button">+</button>
                </div>
                <span>Quantity</span>
              </div>
            </div>
          </li>
        `);

    ulList.append(newItem);
    addCartItemsListener(customerId, cart_float, true);
    updateCartCount(ulList.find("li.price-list").length);
    return newItem;
}
function getProductQuantity(button) {
    var parentDiv = $(button).closest('.dish');
    var inputField = parentDiv.find('.product-qty');
    return parseInt(inputField.val());
}
function getQuantity(button) {
    var parentDiv = $(button).closest('.rate');
    var inputField = parentDiv.find('.product-qty');
    return parseInt(inputField.val());
}
function addToBasket(productId, customerId, quantity) {
    if (customerId == 0) {
        window.location.href = `/login`;
    }
    $.ajax({
        type: "POST",
        url: "/FoodMenu?handler=AddToBasket",
        data: JSON.stringify({
            CustomerId: customerId,
            ProductId: productId,
            Amount: quantity,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val(),
        },
    })
        .done(function (data) {
            const existingItem = ulList.find(`li[data-product="${productId}"]`);
            if (existingItem.length) {
                const itemQty = parseInt(existingItem.find(".product-qty").val());
                const updatedQty = itemQty + quantity;
                existingItem.find(".product-qty").val(updatedQty);
                updateSubtotal(existingItem);
            } else {
                createCartItem(customerId, productId, data.product.productName, data.product.unitPrice, quantity, data.product.productImage);
            }
            updateTotalPrice(cart_float);
            toastr.success("Item added to cart", "", { positionClass: "toast-bottom-right" });
        })
        .fail(function (error) {
            console.error("Error:", error.message);
            toastr.error("Connection error", "", { positionClass: "toast-bottom-right" });
        });
}

function updateCartItem(customerId, productId, quantity) {
    $.ajax({
        type: "POST",
        url: "/FoodMenu?handler=UpdateCart",
        data: JSON.stringify({
            CustomerId: customerId,
            ProductId: productId,
            Amount: quantity,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val(),
        },
        success: function (data) {
            //toastr.success("Cart updated", "", { positionClass: "toast-bottom-right" });
        },
        error: function (error) {
            console.error("Error:", error);
            toastr.error("Connection error", "", { positionClass: "toast-bottom-right" });
        },
    });
}

function redirectToCheckout(customerId) {
    if (customerId == 0) {
        window.location.href = `/login`
    }
    window.location.href = `/checkout`
}