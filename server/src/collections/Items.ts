import { CollectionConfig } from 'payload/types'

const Items: CollectionConfig = {
  slug: 'items',  
  admin: {
    useAsTitle: 'name',
  },
  fields: [
    {
      name: "name",
      type: "text"
    },
    {
      name: "amount",
      type: "number"
    },
    {
      name: "unit",
      type: "text"
    }
  ],
}

export default Items
