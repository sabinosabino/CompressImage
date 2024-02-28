using ImageMagick;

namespace CompressImg.Compress
{
    public class CompressImg
    {
        public byte[] Converter(byte[] imageBytes, int maxquality)
        {
            try
            {
                if (imageBytes.Length > maxquality * 1024)
                {
                    byte[] optimizedImageBytes = OptimizeImage(imageBytes, maxquality * 1024);
                    return optimizedImageBytes;
                }
                return imageBytes;
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

                        int initialQuality = 85;
                        int qualityStep = 5;
                        int currentQuality = initialQuality;

                        MagickFormat outputFormat = MagickFormat.Jpg;
                        int outputQuality = initialQuality;

                        while (true)
                        {
                            using (var tempStream = new MemoryStream())
                            {
                                image.Quality = currentQuality;
                                image.Format = outputFormat;
                                image.Write(tempStream);

                                if (tempStream.Length <= maxSizeBytes || currentQuality <= 5)
                                {
                                    return tempStream.ToArray();
                                }

                                currentQuality -= qualityStep;
                            }
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
