using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeautyClock.Logic
{
    /// <summary>
    /// 画像処理クラス
    /// </summary>
    public static class ImageProcessing
    {
        // 画像サイズ
        const int WIDTH = 590;
        const int HEIGHT = 490;

        // RGBA
        const int LINE = 4;

        // 色情報
        const int COLOR_WHITE = 255;
        const int COLOR_BLACK = 0;

        // 二値化閾値
        const int THRESH = 50;

        /// <summary>
        /// 画像の時刻を指定した時刻に置き換える
        /// </summary>
        /// <param name="image">画像データ</param>
        /// <param name="time">置き換え時刻</param>
        /// <returns>置き換え画像データ</returns>
        public static Stream ReplaceNowTime(Stream image, DateTime time)
        {
            try
            {
                byte[,] data = new byte[WIDTH, HEIGHT];

                // StreamからByteに変換
                data = streamToByteImage(image);
                // 二値化
                //data = formationOfTwoValues(data, THRESH);

                // TODO
                image = byteToStreamImage(image, data);

                return image;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Stream画像データをByteデータに変換する
        /// </summary>
        /// <param name="image">Stream画像データ</param>
        /// <returns>Byteデータ</returns>
        private static byte[,] streamToByteImage(Stream image)
        {
            byte[,] data = new byte[WIDTH, HEIGHT];

            try 
            {
                int b;
                int addByte = 0;
                int count = 0;
                int width = 0;
                int height = 0;
                // 1バイトずつ読み込み
                while ((b = image.ReadByte()) != -1)
                {
                    // RGBデータの場合(最後のAは不要)
                    if (count < LINE - 1)
                    {
                        // RGBを加算していく
                        addByte += b;
                        count++;
                    }
                    // RGBデータでない場合(最後のAの場合)
                    else 
                    {
                        // 色情報をグレースケールに変換
                        data[width, height] = (byte)(addByte / 3);

                        width++;
                        if (width >= WIDTH)
                        {
                            width = 0;
                            height++;
                        }

                        // 初期化
                        addByte = 0;
                        count = 0;
                    }
                }
            }
            catch 
            {
                data = null; 
            }

            return data;
        }

        /// <summary>
        /// ByteデータのRBGをStreamデータへ置き換える
        /// </summary>
        /// <param name="image"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Stream byteToStreamImage(Stream image, byte[,] data)
        {
            int b;
            int count = 0;
            int width = 0;
            int height = 0;
            while ((b = image.ReadByte()) != -1)
            {
                // RGBを書き込み
                if (count != (LINE - 1))
                {
                    image.WriteByte(data[width, height]);
                    count++;
                }
                else
                {
                    count = 0;
                }

                width++;
                if (width >= WIDTH)
                {
                    width = 0;
                    height++;
                }
            }

            return image;
        }

        /// <summary>
        /// 二値化処理
        /// </summary>
        /// <param name="image">二値化対象画像</param>
        /// <param name="thresh">閾値</param>
        /// <returns></returns>
        private static byte[,] formationOfTwoValues(byte[,] image, int thresh)
        {
            // 画像の2値化の実行
            for (int i = 0; i < WIDTH; i++)
                for (int j = 0; j < HEIGHT; j++)
                {
                    // RGB平均値が閾値以下の場合
                    if (image[i, j] <= thresh)
                        // 白色に設定
                        image[i,j] = COLOR_WHITE;
                    // RGB平均値が閾値より大きい場合
                    else
                        // 黒色に設定
                        image[i, j] = COLOR_BLACK;
                }

            return image;
        }

    }
}
