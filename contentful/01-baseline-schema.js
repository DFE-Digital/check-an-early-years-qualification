module.exports = function (migration) {
  const cookiesBanner = migration
    .createContentType("cookiesBanner")
    .name("Cookies Banner")
    .description(
      "Content type for the cookies banner that shows on every page if the user has not chosen their cookie preferences"
    )
    .displayField("cookiesBannerTitle");

  cookiesBanner
    .createField("cookiesBannerTitle")
    .name("CookiesBannerTitle")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  cookiesBanner
    .createField("cookiesBannerContent")
    .name("CookiesBannerContent")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
        ],

        message:
          "Only ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, inline entry, heading 1, heading 2, heading 3, heading 4, heading 5, and heading 6 nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  cookiesBanner
    .createField("acceptButtonText")
    .name("AcceptButtonText")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  cookiesBanner
    .createField("rejectButtonText")
    .name("RejectButtonText")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  cookiesBanner
    .createField("cookiesBannerLinkText")
    .name("CookiesBannerLinkText")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  cookiesBanner
    .createField("acceptedCookiesContent")
    .name("AcceptedCookiesContent")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  cookiesBanner
    .createField("rejectedCookiesContent")
    .name("RejectedCookiesContent")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  cookiesBanner
    .createField("hideCookieBannerButtonText")
    .name("HideCookieBannerButtonText")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  cookiesBanner.changeFieldControl(
    "cookiesBannerTitle",
    "builtin",
    "singleLine",
    {}
  );
  cookiesBanner.changeFieldControl(
    "cookiesBannerContent",
    "builtin",
    "richTextEditor",
    {}
  );
  cookiesBanner.changeFieldControl(
    "acceptButtonText",
    "builtin",
    "singleLine",
    {}
  );
  cookiesBanner.changeFieldControl(
    "rejectButtonText",
    "builtin",
    "singleLine",
    {}
  );
  cookiesBanner.changeFieldControl(
    "cookiesBannerLinkText",
    "builtin",
    "singleLine",
    {}
  );
  cookiesBanner.changeFieldControl(
    "acceptedCookiesContent",
    "builtin",
    "richTextEditor",
    {}
  );
  cookiesBanner.changeFieldControl(
    "rejectedCookiesContent",
    "builtin",
    "richTextEditor",
    {}
  );
  cookiesBanner.changeFieldControl(
    "hideCookieBannerButtonText",
    "builtin",
    "singleLine",
    {}
  );

  const questionPage = migration
    .createContentType("questionPage")
    .name("Question Page")
    .description(
      "Model for storing information relating to questions used within the journey"
    )
    .displayField("question");

  questionPage
    .createField("question")
    .name("Question")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        unique: true,
      },
    ])
    .disabled(false)
    .omitted(false);

  questionPage
    .createField("options")
    .name("Options")
    .type("Array")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: ["option"],
        },
      ],

      linkType: "Entry",
    });

  questionPage
    .createField("ctaButtonText")
    .name("CTA Button Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .defaultValue({
      "en-GB": "Continue",
    })
    .disabled(false)
    .omitted(false);

  questionPage
    .createField("errorMessage")
    .name("Error Message")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .defaultValue({
      "en-GB": "Select an option",
    })
    .disabled(false)
    .omitted(false);

  questionPage
    .createField("additionalInformationHeader")
    .name("Additional Information Header")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  questionPage
    .createField("additionalInformationBody")
    .name("Additional Information Body")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline"],
        message: "Only bold, italic, and underline marks are allowed",
      },
      {
        enabledNodeTypes: ["ordered-list", "unordered-list", "hyperlink"],
        message:
          "Only ordered list, unordered list, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  questionPage.changeFieldControl("question", "builtin", "singleLine", {
    helpText: "This is the question to show on the page",
  });

  questionPage.changeFieldControl("options", "builtin", "entryLinksEditor", {});

  questionPage.changeFieldControl("ctaButtonText", "builtin", "singleLine", {
    helpText:
      "This is the text that appears on the main call to action (CTA) button",
  });

  questionPage.changeFieldControl("errorMessage", "builtin", "singleLine", {
    helpText:
      "This is the message that appears when a user doesn't select an option",
  });

  questionPage.changeFieldControl(
    "additionalInformationHeader",
    "builtin",
    "singleLine",
    {
      helpText:
        "(OPTIONAL) This is the heading for the additional information section",
    }
  );

  questionPage.changeFieldControl(
    "additionalInformationBody",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "(OPTIONAL) This is the body for the additional information section",
    }
  );

  const phaseBanner = migration
    .createContentType("phaseBanner")
    .name("Phase Banner")
    .description("Model to store the phase banner details e.g. Alpha or Beta")
    .displayField("phaseName");
  phaseBanner
    .createField("phaseName")
    .name("PhaseName")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  phaseBanner
    .createField("content")
    .name("Content")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline", "code"],
        message: "Only bold, italic, underline, and code marks are allowed",
      },
      {
        enabledNodeTypes: [
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only ordered list, unordered list, horizontal rule, quote, block entry, asset, link to Url, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  phaseBanner
    .createField("show")
    .name("Show")
    .type("Boolean")
    .localized(false)
    .required(true)
    .validations([])
    .defaultValue({
      "en-GB": true,
    })
    .disabled(false)
    .omitted(false);

  phaseBanner.changeFieldControl("phaseName", "builtin", "singleLine", {});
  phaseBanner.changeFieldControl("content", "builtin", "richTextEditor", {});
  phaseBanner.changeFieldControl("show", "builtin", "boolean", {});

  const navigationLinks = migration
    .createContentType("navigationLinks")
    .name("Navigation Links")
    .description(
      "A collection of internal and/or external links. Allows reordering of links in footer."
    );

  navigationLinks
    .createField("links")
    .name("Links")
    .type("Array")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: ["internalNavigationLink", "externalNavigationLink"],
        },
      ],

      linkType: "Entry",
    });

  navigationLinks.changeFieldControl(
    "links",
    "builtin",
    "entryLinksEditor",
    {}
  );
  const internalNavigationLink = migration
    .createContentType("internalNavigationLink")
    .name("Internal Navigation Link")
    .description(
      "Links to pages inside of our service. To be shown in the footer."
    )
    .displayField("displayText");
  internalNavigationLink
    .createField("displayText")
    .name("Display Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  internalNavigationLink
    .createField("href")
    .name("Href")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  internalNavigationLink
    .createField("openInNewTab")
    .name("Open In New Tab")
    .type("Boolean")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  internalNavigationLink.changeFieldControl(
    "displayText",
    "builtin",
    "singleLine",
    {}
  );
  internalNavigationLink.changeFieldControl(
    "href",
    "builtin",
    "singleLine",
    {}
  );
  internalNavigationLink.changeFieldControl(
    "openInNewTab",
    "builtin",
    "boolean",
    {}
  );
  const externalNavigationLink = migration
    .createContentType("externalNavigationLink")
    .name("External Navigation Link")
    .description(
      "Links to services outside of our service. To be shown in the footer."
    )
    .displayField("displayText");
  externalNavigationLink
    .createField("displayText")
    .name("Display Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  externalNavigationLink
    .createField("href")
    .name("Href")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        regexp: {
          pattern:
            "^(ftp|http|https):\\/\\/(\\w+:{0,1}\\w*@)?(\\S+)(:[0-9]+)?(\\/|\\/([\\w#!:.?+=&%@!\\-/]))?$",
          flags: null,
        },
      },
    ])
    .disabled(false)
    .omitted(false);

  externalNavigationLink
    .createField("openInNewTab")
    .name("Open In New Tab")
    .type("Boolean")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  externalNavigationLink.changeFieldControl(
    "displayText",
    "builtin",
    "singleLine",
    {}
  );
  externalNavigationLink.changeFieldControl(
    "href",
    "builtin",
    "singleLine",
    {}
  );
  externalNavigationLink.changeFieldControl(
    "openInNewTab",
    "builtin",
    "boolean",
    {}
  );
  const cookiesPage = migration
    .createContentType("cookiesPage")
    .name("Cookies Page")
    .description("Content type for the cookies page")
    .displayField("heading");
  cookiesPage
    .createField("heading")
    .name("Heading")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  cookiesPage
    .createField("body")
    .name("Body")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  cookiesPage
    .createField("options")
    .name("Options")
    .type("Array")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false)
    .items({
      type: "Link",

      validations: [
        {
          linkContentType: ["option"],
        },
      ],

      linkType: "Entry",
    });

  cookiesPage
    .createField("buttonText")
    .name("ButtonText")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  cookiesPage
    .createField("successBannerHeading")
    .name("Success Banner Heading")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  cookiesPage
    .createField("successBannerContent")
    .name("Success Banner Content")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  cookiesPage
    .createField("errorText")
    .name("Error Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  cookiesPage.changeFieldControl("heading", "builtin", "singleLine", {});
  cookiesPage.changeFieldControl("body", "builtin", "richTextEditor", {});
  cookiesPage.changeFieldControl("options", "builtin", "entryLinksEditor", {});
  cookiesPage.changeFieldControl("buttonText", "builtin", "singleLine", {});
  cookiesPage.changeFieldControl(
    "successBannerHeading",
    "builtin",
    "singleLine",
    {}
  );
  cookiesPage.changeFieldControl(
    "successBannerContent",
    "builtin",
    "richTextEditor",
    {}
  );
  cookiesPage.changeFieldControl("errorText", "builtin", "singleLine", {});

  const detailsPage = migration
    .createContentType("detailsPage")
    .name("DetailsPage")
    .description(
      "Static Content for the Qualification Details page. (Headings, labels etc)"
    )
    .displayField("mainHeader");

  detailsPage
    .createField("mainHeader")
    .name("MainHeader")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("awardingOrgLabel")
    .name("AwardingOrgLabel")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("levelLabel")
    .name("LevelLabel")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("qualificationNumberLabel")
    .name("QualificationNumberLabel")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("dateAddedLabel")
    .name("DateAddedLabel")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("dateOfCheckLabel")
    .name("DateOfCheckLabel")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("bookmarkHeading")
    .name("BookmarkHeading")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("bookmarkText")
    .name("BookmarkText")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  detailsPage
    .createField("checkAnotherQualificationHeading")
    .name("CheckAnotherQualificationHeading")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  detailsPage
    .createField("checkAnotherQualificationText")
    .name("CheckAnotherQualificationText")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  detailsPage
    .createField("furtherInfoHeading")
    .name("FurtherInfoHeading")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  detailsPage
    .createField("furtherInfoText")
    .name("FurtherInfoText")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
          "entry-hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to asset, inline entry, and link to entry nodes are allowed",
      },
      {
        nodes: {
          "embedded-entry-block": [
            {
              linkContentType: ["govUkInsetText"],
              message: null,
            },
          ],

          "embedded-entry-inline": [
            {
              linkContentType: ["govUkInsetText"],
              message: null,
            },
          ],

          "entry-hyperlink": [
            {
              linkContentType: ["govUkInsetText"],
              message: null,
            },
          ],
        },
      },
    ])
    .disabled(false)
    .omitted(false);

  detailsPage.changeFieldControl("mainHeader", "builtin", "singleLine", {});
  detailsPage.changeFieldControl(
    "awardingOrgLabel",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl("levelLabel", "builtin", "singleLine", {});
  detailsPage.changeFieldControl(
    "qualificationNumberLabel",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl("dateAddedLabel", "builtin", "singleLine", {});
  detailsPage.changeFieldControl(
    "dateOfCheckLabel",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl(
    "bookmarkHeading",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl("bookmarkText", "builtin", "singleLine", {});
  detailsPage.changeFieldControl(
    "checkAnotherQualificationHeading",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl(
    "checkAnotherQualificationText",
    "builtin",
    "richTextEditor",
    {}
  );
  detailsPage.changeFieldControl(
    "furtherInfoHeading",
    "builtin",
    "singleLine",
    {}
  );
  detailsPage.changeFieldControl(
    "furtherInfoText",
    "builtin",
    "richTextEditor",
    {}
  );
  const navigationLink = migration
    .createContentType("navigationLink")
    .name("NavigationLink")
    .description("Links to be used to show in the footer")
    .displayField("displayText");
  navigationLink
    .createField("displayText")
    .name("Display Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  navigationLink
    .createField("href")
    .name("Href")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  navigationLink
    .createField("openInNewTab")
    .name("Open In New Tab")
    .type("Boolean")
    .localized(false)
    .required(false)
    .validations([])
    .defaultValue({
      "en-GB": false,
    })
    .disabled(false)
    .omitted(false);

  navigationLink.changeFieldControl("displayText", "builtin", "singleLine", {});
  navigationLink.changeFieldControl("href", "builtin", "singleLine", {});
  navigationLink.changeFieldControl("openInNewTab", "builtin", "boolean", {});

  const accessibilityStatementPage = migration
    .createContentType("accessibilityStatementPage")
    .name("Accessibility Statement Page")
    .description(
      "Content type for the heading and text on the accessibility statement page"
    )
    .displayField("heading");

  accessibilityStatementPage
    .createField("heading")
    .name("Heading")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  accessibilityStatementPage
    .createField("body")
    .name("Body")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  accessibilityStatementPage.changeFieldControl(
    "heading",
    "builtin",
    "singleLine",
    {}
  );
  accessibilityStatementPage.changeFieldControl(
    "body",
    "builtin",
    "richTextEditor",
    {}
  );

  const option = migration
    .createContentType("option")
    .name("Option")
    .description(
      "This is used by to define what options are available on the question page"
    )
    .displayField("label");

  option
    .createField("label")
    .name("Label")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  option
    .createField("value")
    .name("Value")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        unique: true,
      },
    ])
    .disabled(false)
    .omitted(false);

  option.changeFieldControl("label", "builtin", "singleLine", {
    helpText: "This is the text that appears to the user",
  });

  option.changeFieldControl("value", "builtin", "singleLine", {
    helpText:
      "This is the value that is submitted when someone selects the option",
  });

  const advicePage = migration
    .createContentType("advicePage")
    .name("AdvicePage")
    .description(
      "Used in the journey to provide simple advice to users about certain points in the journey"
    )
    .displayField("heading");

  advicePage
    .createField("heading")
    .name("Heading")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  advicePage
    .createField("body")
    .name("Body")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline"],
        message: "Only bold, italic, and underline marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  advicePage.changeFieldControl("heading", "builtin", "singleLine", {
    helpText: "This is the heading of the advice page",
  });

  advicePage.changeFieldControl("body", "builtin", "richTextEditor", {
    helpText: "This is the content that appears on the help page",
  });

  const govUkInsetText = migration
    .createContentType("govUkInsetText")
    .name("GovUk Inset Text")
    .description(
      "Model to let the content team add inset text html elements into rich text fields"
    )
    .displayField("name");

  govUkInsetText
    .createField("name")
    .name("Name")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  govUkInsetText
    .createField("content")
    .name("Content")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: [
          "bold",
          "italic",
          "underline",
          "code",
          "superscript",
          "subscript",
        ],
        message:
          "Only bold, italic, underline, code, superscript, and subscript marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hr",
          "blockquote",
          "embedded-entry-block",
          "embedded-asset-block",
          "table",
          "hyperlink",
          "entry-hyperlink",
          "asset-hyperlink",
          "embedded-entry-inline",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, horizontal rule, quote, block entry, asset, table, link to Url, link to entry, link to asset, and inline entry nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  govUkInsetText.changeFieldControl("name", "builtin", "singleLine", {});
  govUkInsetText.changeFieldControl("content", "builtin", "richTextEditor", {});
  const startPage = migration
    .createContentType("startPage")
    .name("StartPage")
    .description(
      "This is the start page for the early years qualification journey"
    )
    .displayField("header");
  startPage
    .createField("header")
    .name("Header")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  startPage
    .createField("preCtaButtonContent")
    .name("Pre CTA Button Content")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline"],
        message: "Only bold, italic, and underline marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  startPage
    .createField("ctaButtonText")
    .name("CTA Button Text")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  startPage
    .createField("postCtaButtonContent")
    .name("Post CTA Button Content")
    .type("RichText")
    .localized(false)
    .required(false)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline"],
        message: "Only bold, italic, and underline marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  startPage
    .createField("rightHandSideContentHeader")
    .name("Right Hand Side Content Header")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);

  startPage
    .createField("rightHandSideContent")
    .name("Right Hand Side Content")
    .type("RichText")
    .localized(false)
    .required(true)
    .validations([
      {
        enabledMarks: ["bold", "italic", "underline"],
        message: "Only bold, italic, and underline marks are allowed",
      },
      {
        enabledNodeTypes: [
          "heading-1",
          "heading-2",
          "heading-3",
          "heading-4",
          "heading-5",
          "heading-6",
          "ordered-list",
          "unordered-list",
          "hyperlink",
        ],

        message:
          "Only heading 1, heading 2, heading 3, heading 4, heading 5, heading 6, ordered list, unordered list, and link to Url nodes are allowed",
      },
      {
        nodes: {},
      },
    ])
    .disabled(false)
    .omitted(false);

  startPage.changeFieldControl("header", "builtin", "singleLine", {
    helpText: "This is the heading that appears on the start page",
  });

  startPage.changeFieldControl(
    "preCtaButtonContent",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "This is the content that appears before the main call to action (CTA) button",
    }
  );

  startPage.changeFieldControl("ctaButtonText", "builtin", "singleLine", {
    helpText:
      "This is the text that appears on the main call to action (CTA) button",
  });

  startPage.changeFieldControl(
    "postCtaButtonContent",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "This is the content that appears after the main call to action (CTA) button",
    }
  );

  startPage.changeFieldControl(
    "rightHandSideContentHeader",
    "builtin",
    "singleLine",
    {
      helpText:
        "This is the heading that appears on the right hand side content section",
    }
  );

  startPage.changeFieldControl(
    "rightHandSideContent",
    "builtin",
    "richTextEditor",
    {
      helpText:
        "The content that will appear on the right hand side content section",
    }
  );

  const qualification = migration
    .createContentType("Qualification")
    .name("Qualification")
    .description("Model for storing all the early years qualifications")
    .displayField("qualificationName");

  qualification
    .createField("qualificationId")
    .name("Qualification ID")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([
      {
        unique: true,
      },
    ])
    .disabled(false)
    .omitted(false);

  qualification
    .createField("qualificationName")
    .name("Qualification Name")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("qualificationLevel")
    .name("Qualification Level")
    .type("Integer")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("awardingOrganisationTitle")
    .name("Awarding Organisation Title")
    .type("Symbol")
    .localized(false)
    .required(true)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("fromWhichYear")
    .name("From Which Year")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("toWhichYear")
    .name("To Which Year")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("qualificationNumber")
    .name("Qualification Number")
    .type("Symbol")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("notes")
    .name("Notes")
    .type("Text")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);
  qualification
    .createField("additionalRequirements")
    .name("Additional Requirements")
    .type("Text")
    .localized(false)
    .required(false)
    .validations([])
    .disabled(false)
    .omitted(false);

  qualification.changeFieldControl("qualificationId", "builtin", "singleLine", {
    helpText: "The unique identifier used to reference the qualification",
  });

  qualification.changeFieldControl(
    "qualificationName",
    "builtin",
    "singleLine",
    {
      helpText: "The name of the qualification",
    }
  );

  qualification.changeFieldControl(
    "qualificationLevel",
    "builtin",
    "numberEditor",
    {
      helpText: "The level of the qualification",
    }
  );

  qualification.changeFieldControl(
    "awardingOrganisationTitle",
    "builtin",
    "singleLine",
    {
      helpText: "The name of the awarding organisation",
    }
  );

  qualification.changeFieldControl("fromWhichYear", "builtin", "singleLine", {
    helpText:
      "The date from which the qualification is considered full and relevant",
  });

  qualification.changeFieldControl("toWhichYear", "builtin", "singleLine", {
    helpText:
      "The date on which the qualification stops being considered full and relevant",
  });

  qualification.changeFieldControl(
    "qualificationNumber",
    "builtin",
    "singleLine",
    {
      helpText: "The number of the qualification",
    }
  );

  qualification.changeFieldControl("notes", "builtin", "multipleLine", {
    helpText: "The corresponding notes for the qualification",
  });

  qualification.changeFieldControl(
    "additionalRequirements",
    "builtin",
    "multipleLine",
    {
      helpText: "The additional requirements for the qualification",
    }
  );
};
