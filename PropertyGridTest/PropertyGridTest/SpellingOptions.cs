using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PropertyGridTest {

    [DescriptionAttribute("¦Û­q¿ï¶µ")]
    [TypeConverterAttribute(typeof(SpellingOptionsConverter))]
    public class SpellingOptions {




        [DefaultValueAttribute(true)]
        public bool SpellCheckWhileTyping {
            get { return spellCheckWhileTyping; }
            set { spellCheckWhileTyping = value; }
        }
        private bool spellCheckWhileTyping = true;

        [DefaultValueAttribute(false)]
        public bool SpellCheckCAPS {
            get { return spellCheckCAPS; }
            set { spellCheckCAPS = value; }
        }
        private bool spellCheckCAPS = false;

        [DefaultValueAttribute(true)]
        public bool SuggestCorrections {
            get { return suggestCorrections; }
            set { suggestCorrections = value; }
        }
        private bool suggestCorrections = true;
    }
}
