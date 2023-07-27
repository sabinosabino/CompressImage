using ImageMagick;

namespace CompressImg.Compress
{
    public class CompressImg
    {
        public byte[] Converter(byte[] imageBytes, int maxquality)
        {
            try
            {
                byte[] optimizedImageBytes = OptimizeImage(imageBytes, maxquality*1024);
                return optimizedImageBytes;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a compressão da imagem: " + e.Message);
            }
        }
        private byte[] OptimizeImage(byte[] imageBytes, long maxSizeBytes)
        {
            try
            {
                using (var imageStream = new MemoryStream(imageBytes))
                {
                    using (MagickImage image = new MagickImage(imageStream))
                    {
                        image.Resize(new MagickGeometry(800, 600));

                        int desiredQuality = 85;

                        while (true)
                        {
                            byte[] tempBytes = image.ToByteArray(MagickFormat.Jpg);

                            if (tempBytes.Length <= maxSizeBytes || desiredQuality < 5)
                            {
                                return tempBytes;
                            }

                            desiredQuality -= 5;
                            image.Quality = desiredQuality;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }


        }

    }
}
