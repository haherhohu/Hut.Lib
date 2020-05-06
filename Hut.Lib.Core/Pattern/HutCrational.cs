using System;
using Hut;

namespace Hut
{
    // 비슷한 타입의 무언가를 그냥 생성 T 는 항상 부모 클래스이거나 인터페이스여야.
    // animal->lion, cheetah, bison etc.
    public class HutAbstractFactory<T> where T : class, new()
    {
        public T Create()
        {
            return new T();
        }
    }

    // 빌더 패턴의 대상이 되는 건물
    public interface IHutBuilding
    {
        T Initialize<T>();
    }

    // 건축가
    // 내부적으로 일정한 무언가-해줘야 할 일이 있음(matrix, vector 뭐 그런거)
    public class HutAbstractBuilder<T> where T : IHutBuilding, new()

    {
        public T Build()
        {
            return new T().Initialize<T>(); // with init
        }
    }
}