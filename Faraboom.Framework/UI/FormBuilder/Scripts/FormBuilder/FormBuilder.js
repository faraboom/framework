var fb = {
    behaviours: {},
    transforms: {}
};
$(document)
    .on("focus", ".fb-behaviour", function () {
        var behaviour = $(this).data("fb-behaviour");
        if (fb.behaviours[behaviour]) {
            fb.behaviours[behaviour](this);
        }
    })
    .on("change", ".fb-choices input.fb-choice-selector", function () { //unchecked choice radios
        var choiceArea = $(this).closest(".fb-choice");
        var choices = choiceArea.closest(".fb-choices");
        choices
            .find("> .fb-choice")
            .find(":input")
            .not(choices.find(".fb-choice-selector")
                .not(choices.find(".fb-choices .fb-choice-selector")))
            .attr("disabled", "disabled").each(function () {
                $("span[data-valmsg-for='" + $(this).attr("name") + "']").css("display", "none");
                if ($.validator) {
                    $.validator.defaults.unhighlight(this);
                }
            });
        var myInputs = choiceArea.find(":input").not(choiceArea.find(".fb-choice input"));
        myInputs.attr("disabled", null).each(function () {
            if ($("span[data-valmsg-for='" + $(this).attr("name") + "']").css("display", "").hasClass("field-validation-error")) {
                if ($.validator) {
                    $.validator.defaults.highlight(this);
                }
            }

        });

        var childChoices = choiceArea.find(".fb-choice").not(choiceArea.find(".fb-choice .fb-choice"));
        childChoices.find(".fb-choice-selector").not(childChoices.find(".fb-choices .fb-choice-selector"))
            .attr("disabled", null).not("[checked!='checked']").trigger("change");
    })
    .on("click", ".fb-choices .fb-choice", function (e) {
        if ($(e.target).parents().index($(this)) >= 0) {
            var option = $(this).find("> * > .fb-choice-selector[disabled!='disabled'][checked!='checked']");
            if (option.length) {
                var choicesArea = $(this).closest(".fb-choices-area");
                var picker = choicesArea.find(".fb-choice-picker").not(choicesArea.find(".fb-choices-area .fb-choice-picker"));
                if (picker.length) {
                    picker.find("option:eq(" + $(this).index() + ")").attr("selected", "selected").trigger("change");
                } else {
                    option.attr("checked", "checked").trigger("change");
                }
                e.stopPropagation();
                $(e.target).click();
            }
        }
    })
    .on("change", ".fb-choice-picker", function () {
        var choices = $(this).closest(".fb-choices-area").find("> .fb-choices");
        var radios = choices.find(".fb-choice-selector")
            .not(choices.find(".fb-choices .fb-choice-selector"));
        radios.closest(".fb-choice").hide();
        $(radios[$(this).val()]).attr("checked", "checked").trigger("change").closest(".fb-choice").show();

    });

$.extend(fb.behaviours, {},
    {
        datepicker: function (t) {
            if (!$(t).hasClass("hasDatepicker") && $.datepicker && !$(t).attr("readonly")) {
                // TODO: a more comprehensive switch of .NET to jQuery formats. REF: http://docs.jquery.com/UI/Datepicker/formatDate
                var format = $(t).data("fb-format");
                switch (format) {
                    case "dd MMM yyyy":
                        format = "dd M yy";
                        break;
                    case "dd/MM/yyyy":
                        format = "dd/mm/yy";
                        break;
                }
                $(t).datepicker({ showOn: "focus", dateFormat: format }).focus();
                return true;
            }
            return false;
        },
        datetimepicker: function (t) {
            if (!$(t).hasClass("hasDatepicker") && $.fn.datetimepicker && !$(t).attr("readonly")) {
                $(t).datetimepicker({ showOn: "focus" }).focus();
                return true;
            }
            return false;
        }
    }
);

//Collections
$.extend(fb.transforms,
    { remove: function ($el) { $el.remove(); } },
    {
        swap: function ($el1, $el2) {
            $el1.before($el2);
        }
    });

$(document).ready(function () {
    function newId() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    $(document).on("click", ".fb-add-item", function (e) {
        var encodedForm = $(this).closest("li").find("div.new-fb-collection-outer-container");
        var contentsOfEncodedForm = encodedForm.html();
        var decodedForm = htmlDecode(contentsOfEncodedForm);
        var $form = $($('<div/>').html(decodedForm));

        var modelName = $(this).data("modelname");
        var newObject = $('<li draggable="true" id="listitem-' + newId() + '">').append($form.children().clone());
        newObject.find("> *").css("display", "");

        var newIndex = newId(); // $(this).closest("ul").children().length - 1;
        var renumber = function (index, attr) {
            if (!attr) return attr;
            return modelName + "[" + newIndex + "]." + attr;
        };



        $(newObject).insertBefore($(this).closest(".fb-collection").find("> ul").children().last());

        $(":input", newObject).attr("name", renumber).attr("id", renumber);
        $("input[type='hidden']", newObject).first().attr("name", modelName + ".Index").val(newIndex);

        $(newObject).find("[data-valmsg-for]").attr("data-valmsg-for", renumber);

        $form.find(":input").val(null);
        if ($.validator) {
            if ($.validator.unobtrusive.parseDynamicContent) {
                $.validator.unobtrusive.parseDynamicContent(newObject);
            }
        }
        return false;
    })
        .on("click", ".fb-remove-parent", function () {
            fb.transforms.remove($(this).closest("li"));
            return false;
        })
        .on("click", ".fb-move-up", function () {
            fb.transforms.swap($(this).closest("li").prev(), $(this).closest("li"));
            return false;
        })
        .on("click", ".fb-move-down", function () {
            fb.transforms.swap($(this).closest("li"), $(this).closest("li").next(":not(.fb-not-collection-item)"));
            return false;
        })
        .on("dragstart", function (e) {
            $(this).addClass('fb-dragging');
            e.originalEvent.dataTransfer.setData("text", e.srcElement.id);
        })
        .on("dragover", function (event) {
            event.preventDefault();
            event.stopPropagation();
        })
        .on("dragleave", function (event) {
            event.preventDefault();
            event.stopPropagation();
            $(this).removeClass('fb-dragging');
        })
        .on("drop", function (e) {
            e.preventDefault();
            e.stopPropagation();
            var $src = $(document.getElementById(e.originalEvent.dataTransfer.getData("text")))
                .closest("li[draggable=true]");
            var $target = $(e.target).closest("li[draggable=true]");

            if ($target.length && $target[0] !== $src[0] && $target[0].parentElement === $src[0].parentElement) {
                var beforeOrAfter = $src.index() > $target.index();
                $src.remove();
                if (beforeOrAfter) {
                    $src.insertBefore($target);
                } else {
                    $src.insertAfter($target);
                }
                console.log("dropped");
            }
        });
});
if ($.validator) {
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".form-group").addClass("error");
        },
        unhighlight: function (element) {
            $(element).closest(".form-group").removeClass("error");
        }
    });
}
$(document).on("click keydown", "input[type='checkbox']", function () {
    return !($(this).attr("readonly"));
});

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}