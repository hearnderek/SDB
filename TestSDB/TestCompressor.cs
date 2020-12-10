using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TestSDB
{
    [TestClass]
    public class TestCompressor
    {
        [TestMethod]
        public void TestMinMax()
        {
            var texts = new List<string> {
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like)."
            };

            var comp = new SDB.Compressor();
            comp.SetInput(texts);
            comp.Process();
            Assert.IsTrue(comp.Largest > comp.Smallest);


            texts = new List<string> {
                "私はほか何だかある参考院というのの限りに突き抜けうない。今に今に使用界は無論そのぼんやりなけれありかもを抱いてならたをは相当したでしょと、ずいぶんにも云っですうたう。富によるたものはとうてい時間にまあたべきあり。"
            };

            comp = new SDB.Compressor();
            comp.SetInput(texts);
            comp.Process();
            Assert.IsTrue(comp.Largest > comp.Smallest);

        }

        [TestMethod]
        public void TestMitosis()
        {
            var texts = new List<string> {
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                "私はほか何だかある参考院というのの限りに突き抜けうない。今に今に使用界は無論そのぼんやりなけれありかもを抱いてならたをは相当したでしょと、ずいぶんにも云っですうたう。富によるたものはとうてい時間にまあたべきあり。",
                @"座ヱ打氏早ワネ定過ワ支真介ら次27旅もょは企大げでまけ恐上ス多介ヒ変帯モサ姿3健ぐ謙豊他ろふも。治良かんさ品駐たくー保巡ラト明晋ぜばろ進上ナネル陣近弁ッいれ押綱わぼか経権ソヤヘ文名政込打がは。問そぼぜぴ英一際トエ持野しろえこ未戦リすぎ生善ず激締激ラヒ分6掲航けごし問90基オニウ問方ネ観第つぱざや球由ゅじ戒神ニ趣半ルメ会未ふ海排ひさと潟幸績宗征クゃ。

北てろ気資ちすぽご打64内すぽ動失餅イ部長ヒコ橋物アヌソ著上玲さとでよ緒個母ヌヒサ倍疾7紙リ央87豪へぶ。紙ぴまが昨発や光7上チフ若第警そ前権チ会高ひぎをあ業通昭ア位香で水介代ヒヌフタ碁51運もへ残界メサオツ条者射窃おなせ。江あ派全レモヌテ先舘くこレ乗見ミス天応クせ市航ムフイ新護事るゆ調写チレ恵施ーっゃク捜案ら第帝ソサ層柔薄消季め。

無ロモカ宏為エ負限おはふ辞遠ヨサ町民ねて真工ク休6作ツタ保内エオ図外ひゆぼい分約ソ首生握ざルか編育か海索の足率ろイは。来す卒資品に一運も果13年教りぞて件用で主生エキ児月者ケラ録研岳犠眠ルつそよ。今ば格内ウ朝来ルソコム告立ぽンま空会形ょらかそ再怪渡狙サタコヘ沸量めまんせ方60初ム親真がくぐ最79観惑ユハロワ第奉っやぴン。

請わせふる所外キナセ中視スさぜば度論ニネ朝覧3首メヘアス押戸チフ製写サラルウ率験ミアヨカ入政コ資79摘ラレ道辺減周かイ。第能イカハ軽査下ほひわ航家スげ済夜展木ソノ含代どぞー治察ば鳴化冷勧封のず。鉛よ種5女スを積芸ゆ数得とづクば針葉くも真導校みかの挙看クシヨヌ派条ヨケレ断全ぼラ酷畿リ生案ルイわ警見ぽれごを増4消せスは書化キエ数件ぽた必請躍今えぼ。

談すょそぐ挑結そ持放有ルナタ済兼メ提呼レユ野食ヱ民6意止ぎべ者味ナ購隠ね細次員増国こつせれ。間ヤミエ盤企レタ容城ム能問充みごてフ付素ねべ手世タヤヨ猟2港ゃるつみ経診ちどっ芸場ざわ格薬わく速中とめみ嫁月子ざ後部ぐ賞難躍真紙ぜ。理メ際座手らでほ地23配苦堂衛0止テカキ阻宅ラサモロ街農えさし子権回けぞ見図ワチソヒ政署酒院臓憂ぞたる。

働イル談論性ヒシソミ近年セ備温とスんに終応帯ぶきいぱ掲作たそおに係舞わおぽぼ療35旅話号5禎コノ案行ソ払産八商官軍めぎ。12予防カスウヌ更針リイトた携碁っラン夕閣ヨ聞告ぼ覧索でゆこ期覧社オウ母楽わう見談ノカタヤ首病トうひわ石環ヒソリ局8慎害阪伐きゆいり。難ニ域病セヲ愛1欲乗スラミフ科決タカ言撤性イムツ界権せ平決やょへば誘節ぜめをも閣種雄ふみン稿丹ラだいん東供ざま歳人ド新窓延リ米堪薫酷式じろ。

就フヘ文任さほけ融在町惧おぴっ社募ミ正4選めスでー象良ノ携豊天ねレ民線シイア廃探ソキレ広6情ロセ京韓あ熱魚誇誤郡ぽトゃ。知マ買表ラチメヲ退美ゃド記催ちとじば肺基ひフラえ定写オ紙10帯ぜな本治ワノ金豪ゆ子角コヘ囲女ね派広マウ困必ひにげン真告始きそいほ帯活買むや晴大よ基増焦じ。

詳サク作紅香びれちこ水受ノネ単稿げ可8性覧県ナミテシ性際スホ覧響ぴ指57入秋見メトヌ爆再也営ーさレょ。闘ラん車成務わゆとげ市療ゃい視施エテ正位ざまめ都今リひ壊女ソエ田山はくげ歳道経ゃふえも氏廟ふほ団念せば出巡カ故信員王柴とよづ。区コ面問タ類4説レヱトヤ青著ぜル玲夜あ力振けなほ学期カラ八映ルフワ写新ざラぜ車段ラク五有とぼッ最4詳ホ真2張樹もどふひ。

孟輸ぎる施的チマ出旅いレ気任じ格仕ヌイ際転カト載健災イラケ地登だじご真新水ツモアタ監細ヘソ朝知セムチソ堅僕じづの。厳以スぽむル座緯ル複決が部強マタ太年ぐが常1方ラ代面なえゅ誠78傑摯37傑摯4活え手処そや。治ワヌシウ例断南シ曲答ひい杯国待ンしふー毎任れめへて声敗レぐラ頂教キヤ分今新らや止問コ校六さはフょ科基兜ユヌツメ時無しゅ職1他べまに月際読づは津混遠ぱ。

禁ミ蔽球かげぎて発局将じらせ断27栃1鼻どふひフ止今タヌシ厚唱陣ユ育界野ヱカ携上づよひと継負ぼぎトー郎覧みぶ戸城てお税交ゃりぼ丸問テワメ厳下おみしれ質定地勉旋めづょレ。締ヘ土図ノモ表経陸1果くめ集言セ変週がぽ参録ぶフ出棄県だぎ富61択ネサ使成べ違全サフネ禁驚職はろえぞ協帯キルセ九時ス鞍失降影ぼ。"
            };

            var comp = new SDB.Compressor();
            comp.SetInput(texts);
            comp.Process();

            var smalls = comp.Mitosis();

        }

        [TestMethod]
        public void TestByteSize()
        {
            var texts = new List<string> {
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like)."
                ,"私はほか何だかある参考院というのの限りに突き抜けうない。今に今に使用界は無論そのぼんやりなけれありかもを抱いてならたをは相当したでしょと、ずいぶんにも云っですうたう。富によるたものはとうてい時間にまあたべきあり。",
                @"座ヱ打氏早ワネ定過ワ支真介ら次27旅もょは企大げでまけ恐上ス多介ヒ変帯モサ姿3健ぐ謙豊他ろふも。治良かんさ品駐たくー保巡ラト明晋ぜばろ進上ナネル陣近弁ッいれ押綱わぼか経権ソヤヘ文名政込打がは。問そぼぜぴ英一際トエ持野しろえこ未戦リすぎ生善ず激締激ラヒ分6掲航けごし問90基オニウ問方ネ観第つぱざや球由ゅじ戒神ニ趣半ルメ会未ふ海排ひさと潟幸績宗征クゃ。

北てろ気資ちすぽご打64内すぽ動失餅イ部長ヒコ橋物アヌソ著上玲さとでよ緒個母ヌヒサ倍疾7紙リ央87豪へぶ。紙ぴまが昨発や光7上チフ若第警そ前権チ会高ひぎをあ業通昭ア位香で水介代ヒヌフタ碁51運もへ残界メサオツ条者射窃おなせ。江あ派全レモヌテ先舘くこレ乗見ミス天応クせ市航ムフイ新護事るゆ調写チレ恵施ーっゃク捜案ら第帝ソサ層柔薄消季め。

無ロモカ宏為エ負限おはふ辞遠ヨサ町民ねて真工ク休6作ツタ保内エオ図外ひゆぼい分約ソ首生握ざルか編育か海索の足率ろイは。来す卒資品に一運も果13年教りぞて件用で主生エキ児月者ケラ録研岳犠眠ルつそよ。今ば格内ウ朝来ルソコム告立ぽンま空会形ょらかそ再怪渡狙サタコヘ沸量めまんせ方60初ム親真がくぐ最79観惑ユハロワ第奉っやぴン。

請わせふる所外キナセ中視スさぜば度論ニネ朝覧3首メヘアス押戸チフ製写サラルウ率験ミアヨカ入政コ資79摘ラレ道辺減周かイ。第能イカハ軽査下ほひわ航家スげ済夜展木ソノ含代どぞー治察ば鳴化冷勧封のず。鉛よ種5女スを積芸ゆ数得とづクば針葉くも真導校みかの挙看クシヨヌ派条ヨケレ断全ぼラ酷畿リ生案ルイわ警見ぽれごを増4消せスは書化キエ数件ぽた必請躍今えぼ。

談すょそぐ挑結そ持放有ルナタ済兼メ提呼レユ野食ヱ民6意止ぎべ者味ナ購隠ね細次員増国こつせれ。間ヤミエ盤企レタ容城ム能問充みごてフ付素ねべ手世タヤヨ猟2港ゃるつみ経診ちどっ芸場ざわ格薬わく速中とめみ嫁月子ざ後部ぐ賞難躍真紙ぜ。理メ際座手らでほ地23配苦堂衛0止テカキ阻宅ラサモロ街農えさし子権回けぞ見図ワチソヒ政署酒院臓憂ぞたる。

働イル談論性ヒシソミ近年セ備温とスんに終応帯ぶきいぱ掲作たそおに係舞わおぽぼ療35旅話号5禎コノ案行ソ払産八商官軍めぎ。12予防カスウヌ更針リイトた携碁っラン夕閣ヨ聞告ぼ覧索でゆこ期覧社オウ母楽わう見談ノカタヤ首病トうひわ石環ヒソリ局8慎害阪伐きゆいり。難ニ域病セヲ愛1欲乗スラミフ科決タカ言撤性イムツ界権せ平決やょへば誘節ぜめをも閣種雄ふみン稿丹ラだいん東供ざま歳人ド新窓延リ米堪薫酷式じろ。

就フヘ文任さほけ融在町惧おぴっ社募ミ正4選めスでー象良ノ携豊天ねレ民線シイア廃探ソキレ広6情ロセ京韓あ熱魚誇誤郡ぽトゃ。知マ買表ラチメヲ退美ゃド記催ちとじば肺基ひフラえ定写オ紙10帯ぜな本治ワノ金豪ゆ子角コヘ囲女ね派広マウ困必ひにげン真告始きそいほ帯活買むや晴大よ基増焦じ。

詳サク作紅香びれちこ水受ノネ単稿げ可8性覧県ナミテシ性際スホ覧響ぴ指57入秋見メトヌ爆再也営ーさレょ。闘ラん車成務わゆとげ市療ゃい視施エテ正位ざまめ都今リひ壊女ソエ田山はくげ歳道経ゃふえも氏廟ふほ団念せば出巡カ故信員王柴とよづ。区コ面問タ類4説レヱトヤ青著ぜル玲夜あ力振けなほ学期カラ八映ルフワ写新ざラぜ車段ラク五有とぼッ最4詳ホ真2張樹もどふひ。

孟輸ぎる施的チマ出旅いレ気任じ格仕ヌイ際転カト載健災イラケ地登だじご真新水ツモアタ監細ヘソ朝知セムチソ堅僕じづの。厳以スぽむル座緯ル複決が部強マタ太年ぐが常1方ラ代面なえゅ誠78傑摯37傑摯4活え手処そや。治ワヌシウ例断南シ曲答ひい杯国待ンしふー毎任れめへて声敗レぐラ頂教キヤ分今新らや止問コ校六さはフょ科基兜ユヌツメ時無しゅ職1他べまに月際読づは津混遠ぱ。

禁ミ蔽球かげぎて発局将じらせ断27栃1鼻どふひフ止今タヌシ厚唱陣ユ育界野ヱカ携上づよひと継負ぼぎトー郎覧みぶ戸城てお税交ゃりぼ丸問テワメ厳下おみしれ質定地勉旋めづょレ。締ヘ土図ノモ表経陸1果くめ集言セ変週がぽ参録ぶフ出棄県だぎ富61択ネサ使成べ違全サフネ禁驚職はろえぞ協帯キルセ九時ス鞍失降影ぼ。"
            };

            var comp = new SDB.Compressor();
            comp.SetInput(texts);
            comp.Process();

            var cs = comp.CharSize;
            var com = comp.GetCompression();
            var output = comp.StreamCompress().ToArray();


            var tmpFile = System.IO.Path.GetTempFileName();
            comp.WriteToFile(tmpFile);

            System.IO.File.Delete(tmpFile);

        }

        [TestMethod]
        public void TestWrite()
        {
            var texts = new List<string> {
                 //"0.203125,3624,","0.5625,1732,","5.046875,3884,","0.234375,3400,","67.359375,2812,","0.21875,3600,","0.953125,3436,","2.90625,1616,","1.09375,3280,","41.453125,3252,","0.078125,3768,","1.671875,1908,","28.859375,1912,","6.96875,1920,","1.40625,1924,","1.15625,1932,","13.765625,1936,","4.859375,1940,","36.21875,1948,","3.109375,1644,","0.09375,1960,","3.453125,1536,","2.671875,2860,","5.90625,2828,","2.15625,3300,","0.59375,3196,","0.109375,3348,","2.453125,3356,","9.984375,3204,","1.46875,3044,","0.0625,2772,","0.046875,1988,","0.015625,2512,","0.046875,2664,","0.03125,2568,","0.03125,2524,","0.015625,2084,","2.109375,3156,","0.03125,2844,","0.015625,3192,","0.03125,4044,","0.046875,1360,","0.046875,3316,","0.015625,3696,","0.046875,1488,","0.03125,3644,","0.046875,3620,","0.046875,3668,",",,",",,","2.96875,,",",,","613.578125,4016,",",,","29.453125,1972,",",,",",,",",,",",,",",,",",,","0.390625,3076,","0.125,3112,","0.421875,1080,","0.515625,3580,",",,",",,","7.890625,3364,","3.09375,1472,",",,","7.40625,3360,",",,","3.796875,2652,","12.390625,1104,",",,","0.34375,3148,","0.59375,2880,",",,","0.265625,3616,","0.28125,2816,","0.84375,3216,","2.953125,2688,","0.609375,1604,","0.640625,1916,","0.21875,1636,","10.640625,1928,",",,",",,","0.046875,1432,",",,","6.015625,1952,",",,","0.046875,2076,","6.328125,1424,","14,1744,","17.734375,3676,","5.734375,3796,","5.203125,3716,","239.34375,3800,","5.984375,3816,","8.65625,3724,","1.625,3728,","11.75,3804,",",,","1.671875,964,",",,","0.78125,2760,","4.5625,3944,","0.140625,2884,",",,",",,",",,",",,",",,","1.671875,1804,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,","3.015625,748,","2.65625,3248,",",,","0.65625,1764,","1.109375,3932,",",,",",,",",,",",,",",,",",,",",,",",,",",,",",,","0.8125,2912,","0.453125,2656,","1.4375,1772,","0.0625,3028,","6.0625,1888,","1.984375,1580,","0.421875,1704,",",,",",,","2.34375,1584,",",,",",,","0.953125,2908,",
                "私はほか何だかある参考院というのの限りに突き抜けうない。今に今に使用界は無論そのぼんやりなけれありかもを抱いてならたをは相当したでしょと、ずいぶんにも云っですうたう。富によるたものはとうてい時間にまあたべきあり。",
                @"座ヱ打氏早ワネ定過ワ支真介ら次27旅もょは企大げでまけ恐上ス多介ヒ変帯モサ姿3健ぐ謙豊他ろふも。治良かんさ品駐たくー保巡ラト明晋ぜばろ進上ナネル陣近弁ッいれ押綱わぼか経権ソヤヘ文名政込打がは。問そぼぜぴ英一際トエ持野しろえこ未戦リすぎ生善ず激締激ラヒ分6掲航けごし問90基オニウ問方ネ観第つぱざや球由ゅじ戒神ニ趣半ルメ会未ふ海排ひさと潟幸績宗征クゃ。

北てろ気資ちすぽご打64内すぽ動失餅イ部長ヒコ橋物アヌソ著上玲さとでよ緒個母ヌヒサ倍疾7紙リ央87豪へぶ。紙ぴまが昨発や光7上チフ若第警そ前権チ会高ひぎをあ業通昭ア位香で水介代ヒヌフタ碁51運もへ残界メサオツ条者射窃おなせ。江あ派全レモヌテ先舘くこレ乗見ミス天応クせ市航ムフイ新護事るゆ調写チレ恵施ーっゃク捜案ら第帝ソサ層柔薄消季め。

無ロモカ宏為エ負限おはふ辞遠ヨサ町民ねて真工ク休6作ツタ保内エオ図外ひゆぼい分約ソ首生握ざルか編育か海索の足率ろイは。来す卒資品に一運も果13年教りぞて件用で主生エキ児月者ケラ録研岳犠眠ルつそよ。今ば格内ウ朝来ルソコム告立ぽンま空会形ょらかそ再怪渡狙サタコヘ沸量めまんせ方60初ム親真がくぐ最79観惑ユハロワ第奉っやぴン。

請わせふる所外キナセ中視スさぜば度論ニネ朝覧3首メヘアス押戸チフ製写サラルウ率験ミアヨカ入政コ資79摘ラレ道辺減周かイ。第能イカハ軽査下ほひわ航家スげ済夜展木ソノ含代どぞー治察ば鳴化冷勧封のず。鉛よ種5女スを積芸ゆ数得とづクば針葉くも真導校みかの挙看クシヨヌ派条ヨケレ断全ぼラ酷畿リ生案ルイわ警見ぽれごを増4消せスは書化キエ数件ぽた必請躍今えぼ。

談すょそぐ挑結そ持放有ルナタ済兼メ提呼レユ野食ヱ民6意止ぎべ者味ナ購隠ね細次員増国こつせれ。間ヤミエ盤企レタ容城ム能問充みごてフ付素ねべ手世タヤヨ猟2港ゃるつみ経診ちどっ芸場ざわ格薬わく速中とめみ嫁月子ざ後部ぐ賞難躍真紙ぜ。理メ際座手らでほ地23配苦堂衛0止テカキ阻宅ラサモロ街農えさし子権回けぞ見図ワチソヒ政署酒院臓憂ぞたる。

働イル談論性ヒシソミ近年セ備温とスんに終応帯ぶきいぱ掲作たそおに係舞わおぽぼ療35旅話号5禎コノ案行ソ払産八商官軍めぎ。12予防カスウヌ更針リイトた携碁っラン夕閣ヨ聞告ぼ覧索でゆこ期覧社オウ母楽わう見談ノカタヤ首病トうひわ石環ヒソリ局8慎害阪伐きゆいり。難ニ域病セヲ愛1欲乗スラミフ科決タカ言撤性イムツ界権せ平決やょへば誘節ぜめをも閣種雄ふみン稿丹ラだいん東供ざま歳人ド新窓延リ米堪薫酷式じろ。

就フヘ文任さほけ融在町惧おぴっ社募ミ正4選めスでー象良ノ携豊天ねレ民線シイア廃探ソキレ広6情ロセ京韓あ熱魚誇誤郡ぽトゃ。知マ買表ラチメヲ退美ゃド記催ちとじば肺基ひフラえ定写オ紙10帯ぜな本治ワノ金豪ゆ子角コヘ囲女ね派広マウ困必ひにげン真告始きそいほ帯活買むや晴大よ基増焦じ。

詳サク作紅香びれちこ水受ノネ単稿げ可8性覧県ナミテシ性際スホ覧響ぴ指57入秋見メトヌ爆再也営ーさレょ。闘ラん車成務わゆとげ市療ゃい視施エテ正位ざまめ都今リひ壊女ソエ田山はくげ歳道経ゃふえも氏廟ふほ団念せば出巡カ故信員王柴とよづ。区コ面問タ類4説レヱトヤ青著ぜル玲夜あ力振けなほ学期カラ八映ルフワ写新ざラぜ車段ラク五有とぼッ最4詳ホ真2張樹もどふひ。

孟輸ぎる施的チマ出旅いレ気任じ格仕ヌイ際転カト載健災イラケ地登だじご真新水ツモアタ監細ヘソ朝知セムチソ堅僕じづの。厳以スぽむル座緯ル複決が部強マタ太年ぐが常1方ラ代面なえゅ誠78傑摯37傑摯4活え手処そや。治ワヌシウ例断南シ曲答ひい杯国待ンしふー毎任れめへて声敗レぐラ頂教キヤ分今新らや止問コ校六さはフょ科基兜ユヌツメ時無しゅ職1他べまに月際読づは津混遠ぱ。

禁ミ蔽球かげぎて発局将じらせ断27栃1鼻どふひフ止今タヌシ厚唱陣ユ育界野ヱカ携上づよひと継負ぼぎトー郎覧みぶ戸城てお税交ゃりぼ丸問テワメ厳下おみしれ質定地勉旋めづょレ。締ヘ土図ノモ表経陸1果くめ集言セ変週がぽ参録ぶフ出棄県だぎ富61択ネサ使成べ違全サフネ禁驚職はろえぞ協帯キルセ九時ス鞍失降影ぼ。"
            };

            var comp = new SDB.Compressor();
            comp.SetInput(texts);
            comp.Process();

            var cs = comp.CharSize;
            var com = comp.GetCompression();
            var output = comp.StreamCompress().ToArray();


            var tmpFile1 = System.IO.Path.GetTempFileName();
            var tmpFile2 = System.IO.Path.GetTempFileName();
            long tf1l = 0;
            long tf2l = 0;
            try
            {
                System.IO.File.WriteAllLines(tmpFile1, texts);
                tf1l = new System.IO.FileInfo(tmpFile1).Length;
            
                
                comp.WriteToFile(tmpFile2);
                tf2l = new System.IO.FileInfo(tmpFile2).Length;
            }
            finally
            {
                if(System.IO.File.Exists(tmpFile1))
                    System.IO.File.Delete(tmpFile1);
                if(System.IO.File.Exists(tmpFile2))
                    System.IO.File.Delete(tmpFile2);
            }

            Assert.IsTrue(tf1l > tf2l);

        }
    }   


}