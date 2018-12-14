using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.CameraProperties;

namespace FaceAnalysis
{
    interface ISearchResultHandler
    {
        void UpdateProperties(CameraProperties cameraProperties);
        void Complete();
        Task HandleSearchResult(LikelinessResult result);
    }
}
