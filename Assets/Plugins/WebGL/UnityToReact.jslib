mergeInto(LibraryManager.library, {
  CustomizeStandAreaEvent: function (area_id, width, height) {
    window.dispatchReactUnityEvent(
      	"CustomizeStandAreaEvent",
      	area_id,
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  CustomizeBannerAreaEvent: function (area_id, width, height) {
    window.dispatchReactUnityEvent(
      	"CustomizeBannerAreaEvent",
      	area_id,
      	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  CustomizeCharacterEvent: function (position, character_id) {
    window.dispatchReactUnityEvent(
      "CustomizeCharacterEvent",
      position,
      character_id
    );
  },
});

mergeInto(LibraryManager.library, {
  StandActionPerformedEvent: function (actionJSON) {
    window.dispatchReactUnityEvent(
      "StandActionPerformedEvent",
      UTF8ToString(actionJSON)
    );
  },
});

mergeInto(LibraryManager.library, {
  AuditoriumEnteredEvent: function () {
    window.dispatchReactUnityEvent(
      "AuditoriumEnteredEvent"
    );
  },
});

mergeInto(LibraryManager.library, {
  UploadPDFEvent: function (id_stand) {
    window.dispatchReactUnityEvent(
      "UploadPDFEvent",
      id_stand
    );
  },
});

mergeInto(LibraryManager.library, {
  ExitPreviewStandMenuEvent: function () {
    window.dispatchReactUnityEvent(
      "ExitPreviewStandMenuEvent"
    );
  },
});

mergeInto(LibraryManager.library, {
  ExhibitionCustomizeSponsorScreenEvent: function (width, height) {
    window.dispatchReactUnityEvent(
      	"ExhibitionCustomizeSponsorScreenEvent",
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  ExhibitionCustomizeSponsorDiscEvent: function (area_index, width, height) {
    window.dispatchReactUnityEvent(
      	"ExhibitionCustomizeSponsorDiscEvent",
      	area_index,
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  ExhibitionCustomizeSponsorCylinderEvent: function (sponsor_cylinder_index, width, height) {
    window.dispatchReactUnityEvent(
      	"ExhibitionCustomizeSponsorCylinderEvent",
      	sponsor_cylinder_index,
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  ExhibitionCustomizeSponsorBannerEvent: function (sponsor_banner_index, width, height) {
    window.dispatchReactUnityEvent(
      	"ExhibitionCustomizeSponsorBannerEvent",
      	sponsor_banner_index,
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  UploadCVEvent: function () {
    window.dispatchReactUnityEvent(
      "UploadCVEvent"
    );
  },
});

mergeInto(LibraryManager.library, {
  ApplicationExitEvent: function () {
    window.dispatchReactUnityEvent(
      "ApplicationExitEvent"
    );
  },
});

mergeInto(LibraryManager.library, {
  EntranceCustomizeSponsorScreenEvent: function (area_index, width, height) {
    window.dispatchReactUnityEvent(
      	"EntranceCustomizeSponsorScreenEvent",
      	area_index,
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  EntranceCustomizeSponsorBanner00Event: function (width, height) {
    window.dispatchReactUnityEvent(
      	"EntranceCustomizeSponsorBanner00Event",
	width,
	height
    );
  },
});

mergeInto(LibraryManager.library, {
  EntranceCustomizeSponsorBanner01Event: function (width, height) {
    window.dispatchReactUnityEvent(
      	"EntranceCustomizeSponsorBanner01Event",
	width,
	height
    );
  },
});

