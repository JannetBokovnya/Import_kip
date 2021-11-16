<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VerticalMenu.ascx.cs"
    Inherits="UserControls_Menu_IPhoneMenu_VerticalMenu" %>
<div id="iPhoneFrame" class="OrientationVertical" style="height: 100%;">
    <div style="height: 100%; width:100%; background-color: transparent; position: relative;">
        <a class="backButton" title="Вернуться назад"><%--<span>Вернуться назад</span>--%>
            <div>
                <span>Вернуться назад</span>
            </div>
        </a>

        <div id="RadMenu_iPhone_Content" style="height: 100%;">
            <telerik:RadMenu runat="server" ID="RadMenu1" EnableSelection="false" EnableEmbeddedSkins="false"
                ClickToOpen="true" Flow="vertical" Skin="iPhone" OnClientItemClicked="OnClientItemClicking">
            </telerik:RadMenu>
        </div>
    </div>
</div>
<style type="text/css">
    
</style>
<script type="text/javascript">
    function pageLoad() {
        var RadMenu1 = $find('<%= RadMenu1.ClientID %>');

        var iPodMenu1 = new iPodMenu(RadMenu1);
    }
</script>
<script type="text/javascript">
    (function () {
        $("li.rmItem").has("div").addClass("parent");


        // enforces context over function
        var bind = function (context, name) {
            return function (e) {
                if (e && e.preventDefault) e.preventDefault();
                return context[name].apply(context, arguments);
            };
        };

        // enforces "top" coordinate of element to be within container
        var constrain = function (element, container) {
            var endTop = parseInt($telerik.$(element).css("top"));
            var maxScrollTop = container.offsetHeight - element.offsetHeight;

            if (endTop > 0 || maxScrollTop > 0) {
                $telerik.$(element).animate({
                    top: 0
                }, "slow");
            } else if (endTop < maxScrollTop) {
                $telerik.$(element).animate({
                    top: maxScrollTop
                }, "slow");
            }
        }

        // <dragging>
        var dragStartPos = 0;
        var dragStartTop = 0;

        var dragElement = null;

        var hasDragged = false;

        var dragStart = function (e) {
            if (this.offsetHeight > $get('<%= RadMenu1.ClientID %>').offsetHeight) { /* disable scrolling for menus that do not need it */
                $telerik.$(document.body)
            .bind("mousemove", compositeDrag)
            .bind("mouseup", dragEnd);

                dragElement = $telerik.$(this);

                dragStartPos = e.pageY;
                dragStartTop = parseInt(dragElement.css("top")) || 0;
            }

            e.preventDefault();
            e.stopPropagation();

            hasDragged = false;
        }

        var simpleDrag = function (e) {
            dragElement
        .css("top", dragStartTop - (dragStartPos - e.pageY) + 'px');
        };

        // hit-test
        var compositeDrag = function (e) {
            if (Math.abs(dragStartPos - e.pageY) > 5) /* threshold value */
            {
                simpleDrag(e);

                var container = $get("RadMenu_iPhone_Content");

                $telerik.$("<div id='overlay'></div>")
            .css({
                height: container.offsetHeight,
                width: container.offsetWidth,
                position: "absolute",
                top: 0,
                zIndex: 7100
            })
            .appendTo(container);

                hasDragged = true;

                $telerik.$(document.body)
            .unbind("mousemove", compositeDrag)
            .bind("mousemove", simpleDrag)
            }
        };

        var drag = compositeDrag;

        var dragEnd = function (e) {
            e.preventDefault();
            e.stopPropagation();

            $telerik.$(document.body)
        .unbind("mousemove", simpleDrag)
        .unbind("mousemove", compositeDrag)
        .unbind("mouseup", dragEnd);

            dragElement
        .bind("mousedown", dragStart);

            constrain(dragElement[0], $get('RadMenu_iPhone_Content'));

            $telerik.$('#overlay').remove();
        }
        // </dragging>

        window.iPodMenu = function (RadMenuInstance) {
            var $ = $telerik.$;

            this._animating = false;
            this._menu = RadMenuInstance.get_element();
            this._level = 0;
            this._backButton = $('a.backButton', this._menu.parentNode.parentNode);

            this._backButton.bind("click", bind(this, "navigateBack"));

            RadMenuInstance.add_itemClicking(bind(this, "navigateForward"));

            /* necessary for 2nd level menus */
            RadMenuInstance.add_itemOpening(
            function (sender, args) {
                args.set_cancel(true);
            });

            if (!navigator.userAgent.match(/iPod/i)) {
                /* init drag interface for non-iphone users */
                $(".rmVertical", this._menu)
            .mousedown(dragStart);
            }

            $(".rmTemplateLink", this._menu).click(function (e) {
                if ($(this).attr('href') == '#' || hasDragged)
                    e.preventDefault();
            });
        }

        window.iPodMenu.prototype =
{
    _animateTransition: function (direction, onAnimationEnded) {
        var that = this;

        this._animating = true;

        var finalPosition = parseInt($telerik.getCurrentStyle(this._menu, 'left', '0px')) || 0;

        if (direction == Telerik.Web.UI.SlideDirection.Left)
            finalPosition -= this._menu.offsetWidth;
        else
            finalPosition += this._menu.offsetWidth;

        $telerik.$(this._menu)
            .animate({
                left: finalPosition
            }, 300, "linear",
            function () {
                if (onAnimationEnded)
                    onAnimationEnded();

                that._animating = false;
            });
    },

    hideLastChild: function () {
        this._lastChild =
            $telerik.$(this._lastChild)
                .hide()
                .parent().parent().parent().get(0);
    },

    navigateBack: function (e) {
        if (this._animating) return;

        this._animateTransition(Telerik.Web.UI.SlideDirection.Right, bind(this, "hideLastChild"));

        --this._level;

        if (this._level == 0) {
            this._backButton.fadeOut("fast");
        }
    },

    navigateForward: function (sender, args) {
        if (hasDragged) {
            args.set_cancel(true);
            return;
        }
        else if (this._animating) {
            return;
        }

        var clickedItem = args.get_item();
        var childList = clickedItem.get_childListElement();

        if (childList) {
            // update back button

            this._backButton.fadeIn("fast");

            // show child list on the right

            childList.style.display = "block";

            this._lastChild = childList.parentNode;

            $telerik.$(this._lastChild).css({
                display: "block",
                left: this._menu.offsetWidth + 'px',
                top: -clickedItem.get_element().offsetTop + 'px'
            });

            // animate left

            this._animateTransition(Telerik.Web.UI.SlideDirection.Left);

            this._level++;
        }
        else {
            args.set_cancel(true);
        }
    }
};
    })();
    
</script>
