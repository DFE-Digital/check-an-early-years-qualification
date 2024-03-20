import { ContentfulLivePreview } from '@contentful/live-preview';

const CONFIG = {
  locale: 'en-US',
  fieldIds: ['qualification-name-value'],
};

function SetUpLivePreview(entry) {

  ContentfulLivePreview.init({ locale: CONFIG.locale });

  CONFIG.fieldIds.forEach((fieldId) => {
    setUpField(entry, fieldId);
  })

  function setUpField(entry, fieldId) {
    const callback = (updatedData) => {
      const domElement = document.getElementById(fieldId)
      if (domElement && updatedData.fields && updatedData.fields[fieldId]) {
        // Check if the content is text
        if (typeof updatedData.fields[fieldId] === 'string') {
          domElement.textContent = updatedData.fields[fieldId];
        }
      }
    };

    const unsubscribe = ContentfulLivePreview.subscribe({
      data: entry,
      locale: CONFIG.locale,
      callback,
    });
  }
}

export default SetUpLivePreview;